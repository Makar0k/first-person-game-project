using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public float jumpForce = 20f;
    public Transform selectedCamera;
    public Transform head;
    public Transform rightHand;
    public Transform leftHand;
    public float movementSpeed = 5f;
    public float sprintSpeed = 10f;
    float mouseX = 0;
    float mouseY = 0;
    public float mouseSens = 1f;
    public float gravity = 9.8f;
    float jumpVelocity = 0;
    float gravityMultiplier = 1f;
    bool isCrouching = false;

    [Header("Stairs stepping")]
    public float lowerStairRayHeight = 0.1f;
    public float upperStairRayHeight = 0.5f;
    public float lowerStairRayDist= 0.3f;
    public float upperStairRayDist = 0.3f;
    public float stepHeight = 0.3f;
    bool isGrounded;
    CapsuleCollider mainCollider;
    Vector3 settedHeadPos;
    bool isSprintKeyPressed;
    public bool mouseLocked = false;
    RaycastHit cameraHit;
    [HideInInspector] public List<item> inventory;
    LayerMask cameraMask;

    [Header("Weapon Models")]
    GameObject LeftHandItem;
    [HideInInspector] public byte LHitemID;
    GameObject RightHandItem;
    [HideInInspector] public byte RHitemID;
    private Vector3 itemVelocity = Vector3.zero; // Zero Velocity for camera smooth moving
    public byte[] ammo = new byte[3] { 0, 0, 0 }; //Ammo amount | 0 - Battery | 1 - Pistol | 2 - Shotgun

    void Start()
    {
        cameraMask = LayerMask.GetMask("Player");
        cameraMask = ~cameraMask;
        inventory = new List<item>();
        rb = GetComponent<Rigidbody>();
        mainCollider = GetComponent<CapsuleCollider>();
        settedHeadPos = head.localPosition;
    }
    void LateUpdate()
    {
        
    
    }
    void Update()
    {
        if(mouseLocked == false)
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSens;
            mouseY += Input.GetAxis("Mouse Y") * mouseSens;
        }
        mouseY = Mathf.Clamp(mouseY, -80f, 80f); // View limits

        selectedCamera.rotation = Quaternion.Euler(new Vector3(-mouseY, mouseX, 0)); // Setting Up Camera rotation to mouse Axis
        //Camera Lerp (it will make stairs stepping smooth, but there will be some delay with camera and player itself)
        //selectedCamera.position = Vector3.Lerp(selectedCamera.position, head.transform.position, Time.fixedDeltaTime * 40);
        selectedCamera.position = Vector3.SmoothDamp(selectedCamera.position, head.transform.position, ref itemVelocity, 0.06f);

        if(RightHandItem != null)
        {
            
            RightHandItem.transform.rotation = rightHand.rotation;
            RightHandItem.transform.Rotate(RightHandItem.GetComponent<item>().AdditionalRotation);
            RightHandItem.transform.position = Vector3.Lerp(RightHandItem.transform.position, rightHand.position, Time.deltaTime/Time.fixedDeltaTime);
        }
        
        if(LeftHandItem != null)
        {
            LeftHandItem.transform.rotation =  leftHand.rotation;
            LeftHandItem.transform.Rotate(LeftHandItem.GetComponent<item>().AdditionalRotation);
            LeftHandItem.transform.position = Vector3.Lerp(LeftHandItem.transform.position, leftHand.position, Time.deltaTime/Time.fixedDeltaTime);
        }

        //head.rotation = selectedCamera.rotation; //Head - The object, to which camera is connected but without parenting it

        if(Physics.Raycast(selectedCamera.transform.position, selectedCamera.transform.forward, out cameraHit, 20f, cameraMask))
        {
            if(cameraHit.transform.gameObject.tag == "item")
            {
                if(Input.GetKey("f"))
                {
                    inventory.Add(cameraHit.transform.GetComponent<item>());
                    cameraHit.transform.gameObject.SetActive(false);
                }
            }
        }

        if(Input.GetKeyDown("c"))
        {
            StandPlayerUp(isCrouching);
        }

        if(Input.GetKey(KeyCode.LeftShift) && isGrounded && !isCrouching)
        {
            isSprintKeyPressed = true;
        }
        else
        {
            isSprintKeyPressed = false;
        }

        if(head.localPosition != settedHeadPos)
        {
            head.localPosition = Vector3.Lerp(head.localPosition, settedHeadPos, Time.deltaTime * 8);
        }
    }

    void FixedUpdate()
    {
        Vector3 lookRot = new Vector3(-rb.velocity.x, 0, rb.velocity.z);
        isGrounded = IsGrounded();
        transform.rotation = Quaternion.Euler(new Vector3(0, mouseX, 0));


        if(isGrounded)
        {
            gravityMultiplier = 1f;
            jumpVelocity = 0;
            if(Input.GetKey("space"))
            {
                if(isCrouching)
                {
                    StandPlayerUp(true);
                }
                else
                {
                    jumpVelocity += jumpForce;
                }
            }
        }
        else
        {
            gravityMultiplier = 2f * Time.fixedDeltaTime;
            jumpVelocity -= gravity * gravityMultiplier;    
        }
        rb.velocity = new Vector3(0, jumpVelocity, 0);

        Vector3 movementForce = new Vector3(Input.GetAxis("Horizontal") * (isSprintKeyPressed ? sprintSpeed : movementSpeed), rb.velocity.y, Input.GetAxis("Vertical") * (isSprintKeyPressed ? sprintSpeed : movementSpeed));
        rb.velocity = transform.TransformDirection(movementForce);

        //STAIRS
        Debug.DrawRay(transform.position - new Vector3(0,GetComponent<CapsuleCollider>().height/2 - lowerStairRayHeight,0), transform.forward, Color.green);
        Debug.DrawRay(transform.position - new Vector3(0,GetComponent<CapsuleCollider>().height/2 - upperStairRayHeight,0), transform.forward, Color.red);
        
        if(isGrounded && GetComponent<Rigidbody>().velocity.magnitude > 0f)
        {
            if(Physics.Raycast(transform.position - new Vector3(0,GetComponent<CapsuleCollider>().height/2 - lowerStairRayHeight,0), transform.forward, lowerStairRayDist))
            {
                if(!Physics.Raycast(transform.position - new Vector3(0,GetComponent<CapsuleCollider>().height/2 - upperStairRayHeight,0), transform.forward, upperStairRayDist))
                {
                    transform.position -= new Vector3(0, -stepHeight, 0);
                }
            }
        }
    }
    public void StandPlayerUp(bool arg)
    {
        switch(arg)
        {
            case false:
            {
                isCrouching = true;
                GetComponent<CapsuleCollider>().height = GetComponent<CapsuleCollider>().height/2;
                settedHeadPos = new Vector3(head.localPosition.x, head.localPosition.y - 0.5f, head.localPosition.z);
                break;
            }
            case true:
            {
                RaycastHit hit;
                if(!Physics.Raycast(transform.position, Vector3.up, out hit, GetComponent<Collider>().bounds.extents.y/2 + 0.5f))
                {
                    isCrouching = false;
                    settedHeadPos = new Vector3(head.localPosition.x, head.localPosition.y + 0.5f, head.localPosition.z);
                    GetComponent<CapsuleCollider>().height = GetComponent<CapsuleCollider>().height*2;
                }
                break;
            }
        }
    }
    public bool IsGrounded()
    {
        if(Physics.Raycast(transform.position, -Vector3.up, mainCollider.height/2 + 0.1f)) // Center Ray
            return true;
        if(Physics.Raycast(transform.position + new Vector3(mainCollider.radius/2 - 0.01f, 0, 0), -Vector3.up, mainCollider.height/2 + 0.1f)) // Center + X Radius
            return true;
        if(Physics.Raycast(transform.position  + new Vector3(-mainCollider.radius/2 + 0.01f, 0, 0), -Vector3.up, mainCollider.height/2 + 0.1f)) // Center - X Radius
            return true;
        if(Physics.Raycast(transform.position  + new Vector3(0, 0, mainCollider.radius/2 - 0.01f), -Vector3.up, mainCollider.height/2 + 0.1f)) // Center + Z Radius
            return true;
        if(Physics.Raycast(transform.position  + new Vector3(0, 0, -mainCollider.radius/2 + 0.01f), -Vector3.up, mainCollider.height/2 + 0.1f)) // Center - Z Radius
            return true;

        Debug.DrawRay(transform.position, -Vector3.up, Color.blue);
        Debug.DrawRay(transform.position + new Vector3(mainCollider.radius/2 - 0.01f,0,0), -Vector3.up, Color.blue);
        Debug.DrawRay(transform.position  + new Vector3(-mainCollider.radius/2 + 0.01f,0,0), -Vector3.up, Color.blue);
        Debug.DrawRay(transform.position  + new Vector3(0,0,mainCollider.radius/2  - 0.01f), -Vector3.up, Color.blue);
        Debug.DrawRay(transform.position  + new Vector3(0,0,-mainCollider.radius/2 + 0.01f), -Vector3.up, Color.blue);

        return false;    
    }
    public void TakeItemInLeftHand(int id)
    {
        if(inventory.Count < id + 1) return;
        Destroy(LeftHandItem);
        LeftHandItem = Instantiate(inventory[id].model, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
        LeftHandItem.SetActive(true);
        LeftHandItem.layer = 6;
        Destroy(LeftHandItem.GetComponent<Rigidbody>());
        LeftHandItem.GetComponent<Collider>().enabled = false;
        LeftHandItem.GetComponent<item>().currentHand = 1;
        LeftHandItem.GetComponent<item>().supplyCount = inventory[id].supplyCount;
        if(id == RHitemID)
        {
            Destroy(RightHandItem);
        }
        LHitemID = (byte)id;
    }
    public void TakeItemInRightHand(int id)
    {
        if(inventory.Count < id + 1) return;
        Destroy(RightHandItem);
        RightHandItem = Instantiate(inventory[id].model, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
        RightHandItem.SetActive(true);
        RightHandItem.layer = 6;
        Destroy(RightHandItem.GetComponent<Rigidbody>());
        RightHandItem.GetComponent<Collider>().enabled = false;
        RightHandItem.GetComponent<item>().currentHand = 2;
        RightHandItem.GetComponent<item>().supplyCount = inventory[id].supplyCount;
        if(id == LHitemID)
        {
            Destroy(LeftHandItem);
        }
        RHitemID = (byte)id;
    }
}
