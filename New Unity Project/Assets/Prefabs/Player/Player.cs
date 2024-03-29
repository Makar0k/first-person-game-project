using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public float jumpForce = 20f;
    [SerializeField] protected float maxHealth = 150f;
    [SerializeField] protected float currentHealth = 100f;

    [SerializeField] private Transform selectedCamera;
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
    [SerializeField] private float lowerStairRayHeight = 0.1f;
    [SerializeField] private float upperStairRayHeight = 0.5f;
    [SerializeField] private float lowerStairRayDist= 0.3f;
    [SerializeField] private float upperStairRayDist = 0.3f;
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private bool isGrounded;
    CapsuleCollider mainCollider;
    Vector3 settedHeadPos;
    bool isSprintKeyPressed;
    public bool mouseLocked = false;
    RaycastHit cameraHit;
    [HideInInspector] public List<item> inventory;
    LayerMask cameraMask;

    [Header("Weapon Models")]
    [HideInInspector] public GameObject LeftHandItem;
    [HideInInspector] public short LHitemID = -1;
    [HideInInspector] public GameObject RightHandItem;
    [HideInInspector] public short RHitemID = -1;
    public GameObject cameraStuff;

    private Vector3 itemVelocity = Vector3.zero; // Zero Velocity for camera smooth moving
    public byte[] ammo = new byte[3] { 0, 0, 0 }; //Ammo amount | 0 - Battery | 1 - Pistol | 2 - Shotgun
    int stepState = 1;
    private float currentSpeed;
    public Transform crosshair;
    [SerializeField] protected GameObject HealthUI;
    private UnityEngine.UI.Slider HealthUiComponent;

    void Start()
    {
        LHitemID = -1;
        RHitemID = -1;
        cameraMask = LayerMask.GetMask("Player");
        cameraMask = ~cameraMask;
        inventory = new List<item>();
        rb = GetComponent<Rigidbody>();
        mainCollider = GetComponent<CapsuleCollider>();
        settedHeadPos = head.localPosition;
        HealthUiComponent = HealthUI.GetComponent<UnityEngine.UI.Slider>();
    }
    void LateUpdate()
    {   
        // Rotation is changing in LateUpdate to avoid Animator rotation change
        if(RightHandItem != null && RHitemID != -1)
        {
            RightHandItem.transform.rotation = Quaternion.Lerp(RightHandItem.transform.rotation, rightHand.rotation * Quaternion.Euler(RightHandItem.GetComponent<item>().AdditionalRotation), Time.deltaTime * 40);
        }
        if(LeftHandItem != null && LHitemID != -1)
        {
            LeftHandItem.transform.rotation = Quaternion.Lerp(LeftHandItem.transform.rotation, leftHand.rotation * Quaternion.Euler(LeftHandItem.GetComponent<item>().AdditionalRotation), Time.deltaTime * 40);
        }
    }
    void Update()
    {
        HealthUiComponent.value = currentHealth/(maxHealth/100);
        currentSpeed = rb.velocity.magnitude;
        if(mouseLocked == false)
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSens;
            mouseY += Input.GetAxis("Mouse Y") * mouseSens;
        }
        mouseY = Mathf.Clamp(mouseY, -80f, 80f); // View limits

        selectedCamera.rotation = Quaternion.Euler(new Vector3(-mouseY, mouseX, 0)); // Setting Up Camera rotation to mouse Axis
        //Camera Lerp (it will make stairs stepping smooth, but there will be some delay with camera and player itself)
        //selectedCamera.position = Vector3.Lerp(selectedCamera.position, head.transform.position, Time.fixedDeltaTime * 40);

        // I have done a code based walking animation for camera
        if(stepState == 1 && isGrounded && currentSpeed > 1f)
        {
            selectedCamera.position = Vector3.SmoothDamp(selectedCamera.position, head.transform.position + new Vector3(0, 0.2f, 0), ref itemVelocity, isSprintKeyPressed ? 0.1f : 0.15f);
            if(((head.transform.position + new Vector3(0, 0.2f, 0)) - selectedCamera.position).y < 0.1f)
            {
                stepState = 2;
            }
        }
        if(stepState == 2  && isGrounded &&  currentSpeed > 1)
        {
            selectedCamera.position = Vector3.SmoothDamp(selectedCamera.position, head.transform.position - new Vector3(0, 0.2f, 0), ref itemVelocity, isSprintKeyPressed ? 0.1f : 0.15f);
            if((selectedCamera.position - (head.transform.position - new Vector3(0, 0.2f, 0))).y < 0.1f)
            {
                stepState = 1;
            }
        }
        if(isGrounded && currentSpeed > 1)
        {
            selectedCamera.position = Vector3.SmoothDamp(new Vector3(selectedCamera.position.x, selectedCamera.position.y, selectedCamera.position.z), new Vector3(head.transform.position.x,selectedCamera.position.y,head.transform.position.z), ref itemVelocity, 0.05f);
        }
        else
        {
            selectedCamera.position = Vector3.SmoothDamp(selectedCamera.position, head.transform.position, ref itemVelocity, 0.05f);
        }
    

        // Items

        if(RightHandItem != null && RHitemID != -1)
        {
            if(RightHandItem.GetComponent<item>().isReloading)
            {
                 RightHandItem.transform.position = Vector3.Lerp(RightHandItem.transform.position, transform.position, Time.deltaTime/Time.fixedDeltaTime);
            }
            else
            {
                RightHandItem.transform.position = Vector3.Lerp(RightHandItem.transform.position, rightHand.transform.TransformPoint(RightHandItem.GetComponent<item>().AdditionalLocalPos), Time.deltaTime/Time.fixedDeltaTime);
            }
        }
        if(LeftHandItem != null && LHitemID != -1)
        {
            if(LeftHandItem.GetComponent<item>().isReloading)
            {
                 LeftHandItem.transform.position = Vector3.Lerp(LeftHandItem.transform.position, transform.position, Time.deltaTime/Time.fixedDeltaTime);
            }
            else
            {
                LeftHandItem.transform.position = Vector3.Lerp(LeftHandItem.transform.position, leftHand.transform.TransformPoint(LeftHandItem.GetComponent<item>().AdditionalLocalPos), Time.deltaTime/Time.fixedDeltaTime);
            }
        }

        //head.rotation = selectedCamera.rotation; //Head - The object, to which camera is connected but without parenting it

        if(Input.GetKeyDown("f"))
        {
            if(Physics.Raycast(selectedCamera.transform.position, selectedCamera.transform.forward, out cameraHit, 5f, cameraMask))
            {
                switch(cameraHit.transform.gameObject.tag)
                {
                    case "interactable":
                    {
                        cameraHit.transform.GetComponent<Interactable>().Interact();
                        break;
                    }
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
        //print("LEFT: " + LHitemID + " RIGHT: " + RHitemID);
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
        rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);

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
                movementSpeed = movementSpeed/2;
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
                    movementSpeed = movementSpeed*2;
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
        //Hide previous item
        if(inventory.Count < id + 1) return;
        if(LeftHandItem != null)
        {
            LeftHandItem.SetActive(false);
        }

        LeftHandItem = inventory[id].model;
        LeftHandItem.SetActive(true);
        LeftHandItem.layer = 6;
        var lhItem = LeftHandItem.GetComponent<item>();
        if(LeftHandItem.transform.childCount != 0)
        {
            for(int i = 0; i > LeftHandItem.transform.childCount; i++)
            {
                LeftHandItem.transform.GetChild(i).gameObject.layer = 6;
            }
        }
        LeftHandItem.GetComponent<Rigidbody>().isKinematic = true;
        LeftHandItem.GetComponent<Collider>().enabled = false;
        lhItem.currentHand = 1;

        if(id == RHitemID)
        {
            RightHandItem.SetActive(false);
            RightHandItem = null;
            RHitemID = -1;
        }
        LHitemID = (short)id;

        if(RightHandItem != null)
        {
            if(lhItem.twoHanded == true)
            {
                RightHandItem.SetActive(false);
                RightHandItem = null;
                RHitemID = -1;
            }
            if(RightHandItem.GetComponent<item>().twoHanded == true)
            {
                RightHandItem.SetActive(false);
                RightHandItem = null;
                RHitemID = -1;
            }
        }
    }
    public void TakeItemInRightHand(int id)
    {
        if(inventory.Count < id + 1) return;
        if(RightHandItem != null)
        {
            RightHandItem.SetActive(false);
        }

        RightHandItem = inventory[id].model;
        RightHandItem.SetActive(true);
        RightHandItem.layer = 6;
        var rItem = RightHandItem.GetComponent<item>();

        if(RightHandItem.transform.childCount != 0)
        {
            for(int i = 0; i < RightHandItem.transform.childCount; i++)
            {
                RightHandItem.transform.GetChild(i).gameObject.layer = 6;
            }
        }
        RightHandItem.GetComponent<Rigidbody>().isKinematic = true;
        RightHandItem.GetComponent<Collider>().enabled = false;

        rItem.currentHand = 2;
        rItem.supplyCount = inventory[id].supplyCount;
        if(id == LHitemID)
        {
            LHitemID = -1;
            LeftHandItem.SetActive(false);
            LeftHandItem = null;
        }
        RHitemID = (short)id;
        if(LeftHandItem != null)
        {
            if(rItem.twoHanded == true)
            {
                LeftHandItem.SetActive(false);
                LeftHandItem = null;
                LHitemID = -1;
            }
            if(LeftHandItem.GetComponent<item>().twoHanded == true)
            {
                LeftHandItem.SetActive(false);
                LeftHandItem = null;
                LHitemID = -1;
            }
        }
    }
    public void remapItemsId()
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            print(i);
            RightHandItem.transform.GetChild(i).gameObject.layer = 6;
        }
    }

}
