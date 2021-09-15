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
    public Sprite defaultItemRamp;

    void Start()
    {
        currentUIitem = -1;
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
            leftItemInfo.GetChild(3).GetComponent<Text>().text = ""+ player.ammo[player.inventory[player.LHitemID].ammoType];
            if(leftItemInfo.gameObject.activeSelf == false)
            {
                leftItemInfo.GetChild(0).GetComponent<Text>().text = player.inventory[player.LHitemID]._name;
                leftItemInfo.GetChild(2).GetComponent<Image>().sprite = ammoTypesIcons[player.inventory[player.LHitemID].ammoType];
                leftItemInfo.gameObject.SetActive(true);
            }
        }
        else if((leftItemInfo.gameObject.activeSelf == true && player.LeftHandItem == null) || isLeftInvActive)
        {
            leftItemInfo.gameObject.SetActive(false);
        }

        if(player.RightHandItem != null && !isRightInvActive)
        {
            rightItemInfo.GetChild(3).GetComponent<Text>().text = ""+ player.ammo[player.inventory[player.RHitemID].ammoType];
            rightItemInfo.GetChild(1).GetComponent<Text>().text = "" + Mathf.Round(player.inventory[player.RHitemID].supplyCount) +"/"+ player.inventory[player.RHitemID].maxSupplyCount;
            if(rightItemInfo.gameObject.activeSelf == false)
            {
                rightItemInfo.GetChild(0).GetComponent<Text>().text = player.inventory[player.RHitemID]._name;
                rightItemInfo.GetChild(2).GetComponent<Image>().sprite = ammoTypesIcons[player.inventory[player.RHitemID].ammoType];
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
        //print(player.LHitemID + " --- " + player.RHitemID);
        // Item Drop
        if(Input.GetKey("g"))
        {
            if(currentUIitem >= 0) 
            {
                print("You are just dropped: " + player.inventory[currentUIitem].name + " ID - " + currentUIitem);
                print(player.LHitemID + " - " + player.RHitemID);
                player.inventory[currentUIitem].model.transform.position = player.head.transform.position;
                player.inventory[currentUIitem].model.SetActive(true);
                player.inventory[currentUIitem].model.GetComponent<item>().supplyCount = player.inventory[currentUIitem].GetComponent<item>().supplyCount;
                if(player.LHitemID == currentUIitem)
                {
                    Destroy(player.LeftHandItem);
                    player.LHitemID = -1;
                }
                if(player.RHitemID == currentUIitem)
                {
                    Destroy(player.RightHandItem);
                    player.RHitemID = -1;
                }
                for(int i = currentUIitem; i <= player.inventory.Count - 1; i++)
                {
                    if(i == player.LHitemID)
                    {
                        player.LHitemID--;
                    }
                    if(i == player.RHitemID)
                    {
                        player.RHitemID--;
                    }
                }
                player.inventory.RemoveAt(currentUIitem);
                updateInventory();
                currentUIitem = -1;
                ShowItemInfo(false);
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
            leftInventoryUI.GetChild(i).GetComponent<Image>().sprite = player.inventory[i].icon;
            rightInventoryUI.GetChild(i).GetComponent<Image>().sprite = player.inventory[i].icon;
            if(player.inventory[i].specialRamp != null)
            {
                leftInventoryUI.GetChild(i).GetChild(0).GetComponent<Image>().sprite = player.inventory[i].specialRamp;
                rightInventoryUI.GetChild(i).GetChild(0).GetComponent<Image>().sprite = player.inventory[i].specialRamp;
            }
            else
            {
                leftInventoryUI.GetChild(i).GetChild(0).GetComponent<Image>().sprite = defaultItemRamp;
                rightInventoryUI.GetChild(i).GetChild(0).GetComponent<Image>().sprite = defaultItemRamp;
            }
        }
        for(int i = 0 + player.inventory.Count; i < 8; i++)
        {
            leftInventoryUI.GetChild(i).GetChild(0).GetComponent<Image>().sprite = defaultItemRamp;
            rightInventoryUI.GetChild(i).GetChild(0).GetComponent<Image>().sprite = defaultItemRamp;
            leftInventoryUI.GetChild(i).GetComponent<Image>().sprite = null;
            rightInventoryUI.GetChild(i).GetComponent<Image>().sprite = null;
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
        itemInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(itemInfoPanel.GetComponent<RectTransform>().sizeDelta.x, LayoutUtility.GetPreferredHeight(itemInfoPanel.GetChild(0).GetComponent<RectTransform>()) + LayoutUtility.GetPreferredHeight(itemInfoPanel.GetChild(1).GetComponent<RectTransform>()) + itemInfoPanel.GetChild(1).GetComponent<RectTransform>().sizeDelta.y - (player.inventory[id].showAdditionalInfo ? 25f : 45f));
        //itemInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(LayoutUtility.GetPreferredWidth(itemInfoPanel.GetChild(1).GetComponent<RectTransform>()) - 30f, itemInfoPanel.GetComponent<RectTransform>().sizeDelta.y);
        if(player.inventory[id].showAdditionalInfo == true)
        {
            itemInfoPanel.GetChild(2).GetComponent<Text>().text = player.inventory[id].supplyCount + "/" + player.inventory[id].maxSupplyCount;
            itemInfoPanel.GetChild(3).GetComponent<Image>().sprite = ammoTypesIcons[player.inventory[id].ammoType];
            itemInfoPanel.GetChild(3).GetComponent<Image>().enabled = true;
        }
        else
        {
            itemInfoPanel.GetChild(2).GetComponent<Text>().text = "";
            itemInfoPanel.GetChild(3).GetComponent<Image>().enabled = false;
        }
    }
    public void ShowItemInfo(bool a)
    {
        itemInfoPanel.gameObject.SetActive(a);
        if(!a)
            currentUIitem = -1;
    }
}
