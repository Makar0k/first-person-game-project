using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour
{
    public float battery = 100f;
    public float batteryHarm = 0.5;
    bool isFLTurnedOn = false;
    Light _light;
    void Start()
    {
        _light = transform.GetChild(0);
    }
    void Update()
    {
        if(GetComponent<item>().currentHand == 1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                
            }
        }
        if(GetComponent<item>().currentHand == 2)
        {
            if(Input.GetMouseButtonDown(1))
            {
                
            }
        }
    }
    void FixedUpdate()
    {
        if(isFLTurnedOn)
        {
            battery -= batteryHarm;
        }
    }
}
