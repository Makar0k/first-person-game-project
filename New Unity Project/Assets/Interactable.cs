using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    protected Player player;
    protected void Awake()
    {
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
    }
    public virtual void Interact()
    {
        // --?
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
