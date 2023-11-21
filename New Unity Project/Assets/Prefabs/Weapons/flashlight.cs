using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : item
{
    public float batteryHarm = 0.005f;
    bool isFLTurnedOn = false;
    Transform _light;
    int itemID;
    public float reloadTime = 5f;
    float reloadTimer;
    item itemComponent;
    void Start()
    {
        _light = transform.GetChild(0);
        currentHand = GetComponent<item>().currentHand;
        itemComponent = GetComponent<item>();
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

        if(reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
        }

        // When reload timer is done
        if(isReloading && reloadTimer <= 0)
        {
            isReloading = false;
            player.ammo[ammoType] -= 1;
            player.inventory[itemID].supplyCount = 100f;
        }

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
                if(player.inventory[itemID].supplyCount <= 0 && player.ammo[player.inventory[itemID].ammoType] > 0 && !isReloading)
                {
                    reloadTimer = reloadTime;
                    isReloading = true;
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
                if(player.inventory[itemID].supplyCount <= 0 && player.ammo[player.inventory[itemID].ammoType] > 0 && !isReloading)
                {
                    reloadTimer = reloadTime;
                    isReloading = true;
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
