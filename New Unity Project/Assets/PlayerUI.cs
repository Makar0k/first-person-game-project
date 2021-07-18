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
    Player player;
    public Transform leftInventoryUI;
    public Transform rightInventoryUI;
    public Transform itemInfoPanel;
    public int currentUIitem;
    [Header("Equiped Items Stuff")]
    public Transform leftItemInfo;
    public Transform rightItemInfo;
    public List<Sprite> ammoTypesIcons;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

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
        // In hand items short info panel update
        if(player.LeftHandItem != null && !isLeftInvActive)
        {
            leftItemInfo.GetChild(1).GetComponent<Text>().text = "" + Mathf.Round(player.inventory[player.LHitemID].supplyCount) +"/"+ player.inventory[player.LHitemID].maxSupplyCount;
            leftItemInfo.GetChild(3).GetComponent<Text>().text = ""+ player.ammo[player.LeftHandItem.GetComponent<item>().ammoType];
            if(leftItemInfo.gameObject.activeSelf == false)
            {
                leftItemInfo.GetChild(0).GetComponent<Text>().text = player.inventory[player.LHitemID]._name;
                leftItemInfo.GetChild(2).GetComponent<Image>().sprite = ammoTypesIcons[player.LeftHandItem.GetComponent<item>().ammoType];
                leftItemInfo.gameObject.SetActive(true);
            }
        }
        else if((leftItemInfo.gameObject.activeSelf == true && player.LeftHandItem == null) || isLeftInvActive)
        {
            leftItemInfo.gameObject.SetActive(false);
        }

        if(player.RightHandItem != null && !isRightInvActive)
        {
            rightItemInfo.GetChild(3).GetComponent<Text>().text = ""+ player.ammo[player.RightHandItem.GetComponent<item>().ammoType];
            rightItemInfo.GetChild(1).GetComponent<Text>().text = "" + Mathf.Round(player.inventory[player.RHitemID].supplyCount) +"/"+ player.inventory[player.RHitemID].maxSupplyCount;
            if(rightItemInfo.gameObject.activeSelf == false)
            {
                rightItemInfo.GetChild(0).GetComponent<Text>().text = player.inventory[player.RHitemID]._name;
                rightItemInfo.GetChild(2).GetComponent<Image>().sprite = ammoTypesIcons[player.RightHandItem.GetComponent<item>().ammoType];
                rightItemInfo.gameObject.SetActive(true);
            }
        }
        else if((rightItemInfo.gameObject.activeSelf == true && player.RightHandItem == null) || isRightInvActive)
        {
            rightItemInfo.gameObject.SetActive(false);
        }

        // Short info about item in inventory
        itemInfoPanel.position = Input.mousePosition;
        if(isLeftInvActive || isRightInvActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.003f;
            player.mouseLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            player.mouseLocked = false;
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
                updateInventory();
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
                updateInventory();
                isRightInvActive = true;
                if(isLeftInvActive == true)
                {
                    isLeftInvActive = false;
                }
            }
        }
    }
    
    public void updateInventory()
    {
        // Ammo count update
        for(int i = 0; i < player.ammo.Length; i++)
        {
            leftInventory.transform.GetChild(1).GetChild(i).GetComponent<Text>().text = "" + player.ammo[i];
            rightInventory.transform.GetChild(1).GetChild(i).GetComponent<Text>().text = "" + player.ammo[i];
        }
        // Inventory items UI update
        for(int i = 0; i < player.inventory.Count; i++)
        {
            leftInventoryUI.GetChild(i).GetChild(0).GetComponent<Image>().sprite = player.inventory[i].icon;
            rightInventoryUI.GetChild(i).GetChild(0).GetComponent<Image>().sprite = player.inventory[i].icon;
        }
    }
    public void CheckIsMouseOverButton(int id)
    {
        ShowItemInfo(true);
        currentUIitem = id;
        if(id + 1 > player.inventory.Count)
        {
            ShowItemInfo(false);
            return;
        }
        itemInfoPanel.GetChild(0).GetComponent<Text>().text = player.inventory[id]._name; // Item Name
        itemInfoPanel.GetChild(1).GetComponent<Text>().text = player.inventory[id].description; // Item Description 
    }
    public void ShowItemInfo(bool a)
    {
        itemInfoPanel.gameObject.SetActive(a);
    }
}
