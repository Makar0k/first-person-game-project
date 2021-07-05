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
    float mouseX = 0;
    float mouseY = 0;
    public float mouseSens = 1f;
    public float gravity = 9.8f;
    float jumpVelocity = 0;
    float gravityMultiplier = 1f;
    bool isCrouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
                head.localPosition = new Vector3(head.localPosition.x, head.localPosition.y - 0.5f, head.localPosition.z);
                GetComponent<CapsuleCollider>().height = GetComponent<CapsuleCollider>().height/2;
            }
            else
            {   
                RaycastHit hit;
                if(!Physics.Raycast(transform.position, Vector3.up, out hit, GetComponent<Collider>().bounds.extents.y/2 + 0.5f))
                {
                    head.localPosition = new Vector3(head.localPosition.x, head.localPosition.y + 0.5f, head.localPosition.z);
                    isCrouching = false;
                    GetComponent<CapsuleCollider>().height = GetComponent<CapsuleCollider>().height*2;
                }
            }
        }
        Debug.DrawRay(transform.position, Vector3.up, Color.green);
        print(GetComponent<Collider>().bounds.extents.y/2);
    }

    void FixedUpdate()
    {   
        if(IsGrounded())
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

        Vector3 movementForce = new Vector3(Input.GetAxis("Horizontal") * movementSpeed, rb.velocity.y, Input.GetAxis("Vertical") * movementSpeed);
        rb.velocity = transform.TransformDirection(movementForce);
    }
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }
}
