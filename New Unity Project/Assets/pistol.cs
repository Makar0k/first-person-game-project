using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistol : MonoBehaviour
{
    public float damage;
    Player player;
    Animator _animator;
    public GameObject fire;
    int currentHand;
    item itemComponent;
    int itemID;
    float reloadTimer;
    public float reloadTime = 1.5f;
    bool isShooting;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        currentHand = GetComponent<item>().currentHand;
        _animator = GetComponent<Animator>();
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

    // Update is called once per frame
    void Update()
    {
        if(player.mouseLocked)
            return;

        if(reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
        }

        // When reload timer is done
        if(itemComponent.isReloading && reloadTimer <= 0)
        {
            itemComponent.isReloading = false;
            if(player.ammo[itemComponent.ammoType] < (byte)itemComponent.maxSupplyCount)
            {
                player.inventory[itemID].supplyCount = player.ammo[itemComponent.ammoType];
                player.ammo[itemComponent.ammoType] = 0;
            }
            else
            {
                player.ammo[itemComponent.ammoType] -= (byte)itemComponent.maxSupplyCount;
                player.inventory[itemID].supplyCount = itemComponent.maxSupplyCount;
            }
        }


        if(currentHand == 1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(player.inventory[itemID].supplyCount <= 0 && player.ammo[player.inventory[itemID].ammoType] > 0 && !itemComponent.isReloading)
                {
                    reloadTimer = reloadTime;
                    itemComponent.isReloading = true;
                }
                if(isShooting == false && player.inventory[itemID].supplyCount > 0)
                {
                    fire.SetActive(true);
                    _animator.SetBool("isShooting", true);
                    player.inventory[itemID].supplyCount -= 1f;
                    isShooting = true;
                    transform.position -= transform.forward * 0.07f;
                }
            }
        }
        if(currentHand == 2)
        {
            if(Input.GetMouseButtonDown(1))
            {
                
                if(player.inventory[itemID].supplyCount <= 0 && player.ammo[player.inventory[itemID].ammoType] > 0 && !itemComponent.isReloading)
                {
                    reloadTimer = reloadTime;
                    itemComponent.isReloading = true;
                }
                if(isShooting == false  && player.inventory[itemID].supplyCount > 0)
                {
                    fire.SetActive(true);
                    _animator.SetBool("isShooting", true);
                    player.inventory[itemID].supplyCount -= 1f;
                    isShooting = true;
                    transform.position -= transform.forward * 0.07f;
                }
            }
        }
    }

    public void shootIsDone()
    {
        isShooting = false;
        fire.SetActive(false);
        _animator.SetBool("isShooting", false);
    }
}
