using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
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
    public GameObject debug;
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
                    transform.position -= transform.forward * 0.5f;
                    ShootFromCamera();
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
                    transform.position += transform.up * 0.5f;
                    ShootFromCamera();
                }
            }
        }
    }

    public void shootIsDone()
    {
        fire.SetActive(false);
        itemComponent.AdditionalRotation.x += 25f;
    }
    public void reloadIsDone()
    {
        isShooting = false;
        itemComponent.AdditionalRotation.x -= 25f;
        fire.SetActive(false);
        
        _animator.SetBool("isShooting", false);
    }

    public void ShootFromCamera()
    {
        Ray ray = Camera.main.ScreenPointToRay(player.crosshair.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform.GetComponent<Rigidbody>() != null && hit.transform.gameObject.tag != "Player")
            {
                hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(50 * damage * (-Camera.main.transform.position + hit.transform.position).normalized, hit.point);          
            }
            if(hit.transform.gameObject.tag == "enemy")
            {
                dummy Enemy =  hit.transform.root.GetComponent<dummy>();
                if(hit.transform.name == "head")
                {    
                    Enemy.damaged += damage;
                    Enemy.health -= damage;
                }

                Enemy.damaged += damage;
                if(Enemy.damaged >= Enemy.stunDamage)
                {
                    Enemy.TurnRagdoll(true);
                    Enemy.stunTimer = Enemy.stunTime;
                    Enemy.damaged = 0;
                }
                
                Enemy.health -= damage;
                if(Enemy.health <= 0)
                {
                    Enemy.TurnRagdoll(true);
                }
                hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(100 * damage * (-Camera.main.transform.position + hit.transform.position).normalized, hit.point);
            }
        }
    }
}

