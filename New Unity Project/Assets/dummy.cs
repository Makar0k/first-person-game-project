using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummy : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    public Transform target;
    Animator animator;
    private Vector3 previousPosition;
    public float curSpeed;
    public bool isRagdoll;
    public float stunTime = 2f;
    public float health = 40;
    [HideInInspector] public float stunTimer = 0f;
    public float stunDamage = 20f;
    public float damaged = 0;
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.enabled = true;
        TurnRagdoll(false);
        transform.GetChild(0).position = transform.position;
        transform.GetChild(0).position -= new Vector3(0, 0.3f, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        // Stun
        if(stunTimer > 0)
        {
            stunTimer -= Time.deltaTime * 2;
        }
        else if(isRagdoll == true && health > 0)
        {
            stunTimer = 0;
            WakeUp();
        }
        // Calculate Speed
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        animator.SetFloat("speed", curSpeed);

        if(!isRagdoll && stunTimer <= 0)
            agent.destination = target.position;
        
        //Debug
        if(Input.GetKey("r"))
        {
            health = 0;
            TurnRagdoll(true);
        }
        if(Input.GetKey("t"))
        {
            health = 40f;
            GetComponent<dummy>().enabled = true;
            TurnRagdoll(false);
        }
        // Death
        if(this.enabled && health <= 0)
        {
            TurnRagdoll(true);
            this.enabled = false;
        }
    }
    public void TurnRagdoll(bool a)
    {
        if(a)
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = !a;
            }
            animator.enabled = false;
            agent.enabled = false;
            isRagdoll = true;
        }
        else
        {
            transform.position = transform.GetChild(0).GetChild(0).GetChild(0).position;
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = !a;
            }
            animator.enabled = true;
            agent.enabled = true;
            transform.GetChild(0).position = transform.position;
            transform.GetChild(0).position -= new Vector3(0, 0.3f, 0);
            isRagdoll = false;
        }
    }
    public void WakeUp()
    {
        TurnRagdoll(false);
        animator.SetBool("isWaking", true);
        agent.speed = 0;
    }
}
