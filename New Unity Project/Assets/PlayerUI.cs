using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Inventory Stuff")]
    public GameObject leftInventory;
    public GameObject rightInventory;
    public bool isLeftInvActive = false;
    public bool isRightInvActive = false;
    Vector3 rightDefaultPosition;
    Vector3 leftDefaultPosition;
    Vector3 rightNotActivePos;
    Vector3 leftNotActivePos;

    void Start()
    {
        rightDefaultPosition = rightInventory.transform.localPosition;
        leftDefaultPosition = leftInventory.transform.localPosition;

        rightNotActivePos = new Vector3(rightDefaultPosition.x + 2000f, rightDefaultPosition.y, rightDefaultPosition.z);
        leftNotActivePos = new Vector3(leftNotActivePos.x -2000f, leftNotActivePos.y, leftNotActivePos.z);

        rightInventory.transform.localPosition = rightNotActivePos;
        leftInventory.transform.localPosition = leftNotActivePos;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLeftInvActive || isRightInvActive)
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.003f;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        //Left Inventory
        if(isLeftInvActive)
        {
            if(leftInventory.transform.localPosition != leftDefaultPosition)
            {
                leftInventory.transform.localPosition = Vector3.Lerp(leftInventory.transform.localPosition, leftDefaultPosition, Time.fixedDeltaTime * Screen.width/100*2);
            }
        }
        else
        {
            if(leftInventory.transform.localPosition != leftNotActivePos)
            {
                leftInventory.transform.localPosition = Vector3.Lerp(leftInventory.transform.localPosition, leftNotActivePos, Time.deltaTime * Screen.width/100*3);
            }
        }
        //Right Inventory
        if(isRightInvActive)
        {
            if(rightInventory.transform.localPosition != rightDefaultPosition)
            {
                rightInventory.transform.localPosition = Vector3.Lerp(rightInventory.transform.localPosition, rightDefaultPosition, Time.fixedDeltaTime * Screen.width/100*3);
            }
        }
        else
        {
            if(rightInventory.transform.localPosition != rightNotActivePos)
            {
                rightInventory.transform.localPosition = Vector3.Lerp(rightInventory.transform.localPosition, rightNotActivePos, Time.deltaTime * Screen.width/100*3);
            }
        }





        //Input
        if(Input.GetKeyDown("q"))
        {
            if(isLeftInvActive == true)
            {
                isLeftInvActive = false;
            }
            else
            {
                isLeftInvActive = true;
                if(isRightInvActive == true)
                {
                    isRightInvActive = false;
                }
            }
        }
        if(Input.GetKeyDown("e"))
        {
            if(isRightInvActive == true)
            {
                isRightInvActive = false;
            }
            else
            {
                isRightInvActive = true;
                if(isLeftInvActive == true)
                {
                    isLeftInvActive = false;
                }
            }
        }
    }
}
