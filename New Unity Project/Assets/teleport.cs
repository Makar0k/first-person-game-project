using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : Interactable
{
    public bool isPressable = false;
    public Transform ObjectToTeleport;
    void Start()
    {
        if(isPressable)
        {
            this.gameObject.tag = "teleport";
            this.GetComponent<Collider>().isTrigger = false;
        }
        else
        {
            this.gameObject.tag = null;
            this.GetComponent<Collider>().isTrigger = true;
        }
    }

    public override void Interact()
    {
        player.transform.position = ObjectToTeleport.position;
    }
}
