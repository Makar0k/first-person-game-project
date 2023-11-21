using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPack : Interactable
{
    public byte ammoType;
    public byte ammoCount;
    
    public override void Interact()
    {
        ammoPack _ammo = transform.GetComponent<ammoPack>();
        player.ammo[_ammo.ammoType] += _ammo.ammoCount;
        Destroy(transform.gameObject);
    }
}
