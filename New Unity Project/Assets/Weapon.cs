using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : item
{
    public float damage;
    protected Animator _animator;
    public GameObject fire;
    float reloadTimer;
    public float reloadTime = 1.5f;
    protected bool isShooting;
    public GameObject debug;
    void Start()
    {
        _animator = GetComponent<Animator>();
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
        if(isReloading && reloadTimer <= 0)
        {
            isReloading = false;
            if(player.ammo[ammoType] < (byte)maxSupplyCount)
            {
                supplyCount = player.ammo[ammoType];
                player.ammo[ammoType] = 0;
            }
            else
            {
                player.ammo[ammoType] -= (byte)maxSupplyCount;
                supplyCount = maxSupplyCount;
            }
        }


        if(currentHand == 1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(supplyCount <= 0 && player.ammo[ammoType] > 0 && !isReloading)
                {
                    reloadTimer = reloadTime;
                    isReloading = true;
                }
                if(isShooting == false && supplyCount > 0)
                {
                    ShootFromCamera();
                }
            }
        }
        if(currentHand == 2)
        {
            if(Input.GetMouseButtonDown(1))
            {
                if(supplyCount <= 0 && player.ammo[ammoType] > 0 && !isReloading)
                {
                    reloadTimer = reloadTime;
                    isReloading = true;
                }
                if(isShooting == false  && supplyCount > 0)
                {
                    ShootFromCamera();
                }
            }
        }
    }

    public virtual void shootIsDone()
    {
        fire.SetActive(false);
    }

    public virtual void Shake()
    {
        transform.position -= transform.forward * 0.5f;
        AdditionalRotation.x += 25f;
    }
    public virtual void Shake(float shakePos, float rotation)
    {
        transform.position -= transform.forward * shakePos;
        AdditionalRotation.x += rotation;
    }

    public void reloadIsDone()
    {
        isShooting = false;
        AdditionalRotation.x -= 25f;
        fire.SetActive(false);
        
        _animator.SetBool("isShooting", false);
    }

    public void ShootFromCamera()
    {
        Shake();
        fire.SetActive(true);
        _animator.SetBool("isShooting", true);
        supplyCount -= 1f;
        isShooting = true;

        Ray ray = Camera.main.ScreenPointToRay(player.crosshair.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f, ~LayerMask.GetMask("Player")))
        {
            if(hit.transform.GetComponent<Rigidbody>() != null && hit.transform.gameObject.tag != "Player")
            {
                hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(50 * damage * (-Camera.main.transform.position + hit.transform.position).normalized, hit.point);          
            }
            if(hit.transform.gameObject.tag == "enemy")
            {
                enemyAI Enemy =  hit.transform.root.GetComponent<enemyAI>();
                Enemy.damaged += damage;
                Enemy.health -= damage;
                hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(100 * damage * (-Camera.main.transform.position + hit.transform.position).normalized, hit.point);
            }
        }
    }
}
