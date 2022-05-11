using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class enemyAI : MonoBehaviour
{
    protected UnityEngine.AI.NavMeshAgent agent;
    public Transform target;
    Animator animator;
    private Vector3 previousPosition;
    public float speed = 2f;
    public float curSpeed;
    public bool isRagdoll;
    public float stunTime = 2f;
    public float health = 40;
    [HideInInspector] public float stunTimer = 0f;
    public float stunDamage = 20f;
    public float damaged = 0;
    public Transform model;
    public Transform pelvis;
    Rigidbody ragdollRb;

    protected void Start()
    {
        ragdollRb = pelvis.GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = model.GetComponent<Animator>();
        animator.enabled = true;
        TurnRagdoll(false);
        agent.speed = speed;
    }

    // Update is called once per frame
    protected void Update()
    {
        // Stun
        if(stunTimer > 0)
        {
            if(ragdollRb.velocity.magnitude < 0.2f)
            {
                stunTimer -= Time.deltaTime * 2;
            }
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
        
        // Death
        if(this.enabled && health <= 0)
        {
            TurnRagdoll(true);
            this.enabled = false;
        }
    }
    public virtual void TurnRagdoll(bool a)
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
            agent.enabled = false;
        }
        else
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = !a;
            }
            animator.enabled = true;
            agent.enabled = true;
            isRagdoll = false;
        }
    }
    
    public virtual void WakeUp()
    {
        transform.position = pelvis.position;
        animator.SetBool("isWaking", true);
        TurnRagdoll(false);
        agent.speed = 0;
    }
}
