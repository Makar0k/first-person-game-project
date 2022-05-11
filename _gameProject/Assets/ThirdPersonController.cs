using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Camera Options")]
    public Camera mainCamera;
    public float camDistance;
    public bool inBattleMode;
    private Vector3 mouseRot;
    RaycastHit cameraHitDown, cameraHitUp, cameraHitLeft, cameraHitRight;

    void Start()
    {
        
    }

    void Update()
    {
        
        // Camera position
        if(!Physics.Raycast(mainCamera.transform.position, -mainCamera.transform.up, out cameraHitDown, 0.1f))
        {
            if(Input.GetAxis("Mouse Y") > 0)
                mouseRot.x -= Input.GetAxis("Mouse Y") * 3;
        }
        if(!Physics.Raycast(mainCamera.transform.position, mainCamera.transform.up, out cameraHitUp, 0.1f))
        {
            if(Input.GetAxis("Mouse Y") < 0)
                mouseRot.x -= Input.GetAxis("Mouse Y") * 3;
        }

        mouseRot.y += Input.GetAxis("Mouse X") * 3;
        switch(inBattleMode)
        {
            case false:
            {
                mainCamera.transform.position = transform.position - Quaternion.Euler(mouseRot) * transform.forward * camDistance;
                mainCamera.transform.LookAt(transform.position);
                break;
            }
            case true:
            {
                break;
            }
        }
    }
}
