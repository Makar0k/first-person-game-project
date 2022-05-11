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
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = transform.GetComponent<enemyAI>().speed;;
        GetComponent<Animator>().SetBool("isWaking", false);
    }
    void WakeUpStart()
    {
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0f;
        GetComponent<Animator>().SetBool("isWaking", false);
    }
}
