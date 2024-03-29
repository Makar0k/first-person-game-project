using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class item : Interactable
{
    public string _name;
    public string description;
    public GameObject model;
    public Sprite icon;
    public Vector3 AdditionalRotation;
    public Vector3 AdditionalLocalPos;
    public float supplyCount = 0;
    public float maxSupplyCount = 0;
    public byte currentHand = 0; //1 - Left | 2 - Right
    public int ammoType = 0;
    public bool isReloading = false;
    public bool twoHanded = false;
    public Sprite specialRamp;
    public bool showAdditionalInfo = false;
    public override void Interact()
    {
        player.inventory.Add(transform.GetComponent<item>());
        transform.gameObject.SetActive(false);
    }
}
