using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public string _name;
    public string description;
    public GameObject model;
    public Sprite icon;
    public Vector3 AdditionalRotation;
    public float supplyCount;
    public byte currentHand = 0; //1 - Left | 2 - Right

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
