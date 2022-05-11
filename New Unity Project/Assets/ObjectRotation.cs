using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    public float Speed;
    float degree;
    public bool x, y, z;
    Vector3 resultDegree;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(x)
        {
            resultDegree.x += 0.1f * Speed;
        }
        if(y)
        {
            resultDegree.y += 0.1f * Speed;
        }
        if(z)
        {
            resultDegree.z += 0.1f * Speed;
        }
        transform.rotation = Quaternion.Euler(resultDegree);
    }
}
