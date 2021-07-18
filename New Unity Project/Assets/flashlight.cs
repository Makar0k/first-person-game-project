using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour
{
    public float batteryHarm = 0.005f;
    bool isFLTurnedOn = false;
    Transform _light;
    Player player;
    int currentHand;
    int itemID;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        _light = transform.GetChild(0);
        currentHand = GetComponent<item>().currentHand;
        if(currentHand == 1)
        {
            itemID = player.LHitemID;
        }
        if(currentHand == 2)
        {
            itemID = player.RHitemID;
        }
    }
    void Update()
    {
        if(player.mouseLocked)
            return;
        if(currentHand == 1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(isFLTurnedOn == false  && player.inventory[itemID].supplyCount > 0)
                {
                    _light.gameObject.SetActive(true);
                    isFLTurnedOn = true;
                }
                else
                {
                    isFLTurnedOn = false;
                    _light.gameObject.SetActive(false);
                }
                if(player.inventory[itemID].supplyCount <= 0 && player.ammo[0] > 0)
                {
                     player.ammo[GetComponent<item>().ammoType] -= 1;
                     player.inventory[itemID].supplyCount = 100f;
                }
            }
        }
        if(currentHand == 2)
        {
            if(Input.GetMouseButtonDown(1))
            {
                if(isFLTurnedOn == false  && player.inventory[itemID].supplyCount > 0)
                {
                    _light.gameObject.SetActive(true);
                    isFLTurnedOn = true;
                }
                else
                {
                    isFLTurnedOn = false;
                    _light.gameObject.SetActive(false);
                }
                if(player.inventory[itemID].supplyCount <= 0 && player.ammo[0] > 0)
                {
                     player.ammo[0] -= 1;
                     player.inventory[itemID].supplyCount = player.inventory[itemID].maxSupplyCount;
                }
            }
        }
    }
    void FixedUpdate()
    {
        if(isFLTurnedOn && player.inventory[itemID].supplyCount > 0)
        {
            player.inventory[itemID].supplyCount -= batteryHarm * Time.timeScale; 
        }
        else
        {
            if(isFLTurnedOn)
            {
                isFLTurnedOn = false;
                _light.gameObject.SetActive(false);
            }
        }
    }
}
