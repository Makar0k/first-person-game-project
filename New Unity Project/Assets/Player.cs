using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public float jumpForce = 20f;
    public Transform selectedCamera;
    public Transform head;
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCollider = GetComponent<CapsuleCollider>();
        settedHeadPos = head.localPosition;
    }

    void Update()
    {
        selectedCamera.position = head.transform.position;
        mouseX += Input.GetAxis("Mouse X") * mouseSens;
        mouseY += Input.GetAxis("Mouse Y") * mouseSens;

        selectedCamera.rotation = Quaternion.Euler(new Vector3(-mouseY, mouseX, 0));
        transform.rotation = Quaternion.Euler(new Vector3(0, mouseX, 0));
        head.rotation = selectedCamera.rotation;

        if(Input.GetKeyDown("c"))
        {
            if(isCrouching == false)
            {
                isCrouching = true;
                GetComponent<CapsuleCollider>().height = GetComponent<CapsuleCollider>().height/2;
                settedHeadPos = new Vector3(head.localPosition.x, head.localPosition.y - 0.5f, head.localPosition.z);
            }
            else
            {   
                RaycastHit hit;
                if(!Physics.Raycast(transform.position, Vector3.up, out hit, GetComponent<Collider>().bounds.extents.y/2 + 0.5f))
                {
                    isCrouching = false;
                    settedHeadPos = new Vector3(head.localPosition.x, head.localPosition.y + 0.5f, head.localPosition.z);
                    GetComponent<CapsuleCollider>().height = GetComponent<CapsuleCollider>().height*2;
                }
            }
        }
        if(Input.GetKey(KeyCode.LeftShift))
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

        if(isGrounded)
        {
            gravityMultiplier = 1f;
            jumpVelocity = 0;
            if(Input.GetKey("space"))
            {
                jumpVelocity += jumpForce;
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
}