using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAnimHandler : MonoBehaviour
{

    void Start()
    {
        
    }

    void WokeUp()
    {
        transform.parent.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 3.5f;
        GetComponent<Animator>().SetBool("isWaking", false);
        transform.parent.position -= new Vector3(0,0.3f,0);
    }
    void WakeUpStart()
    {
        transform.parent.position += new Vector3(0,0.3f,0);
        GetComponent<Animator>().SetBool("isWaking", false);
    }
}
