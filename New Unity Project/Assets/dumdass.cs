using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumdass : enemyAI
{
    public bool isPatrol;
    public float patrolWaitTime;
    byte curWaypoint = 0;
    float pTimer;
    public List<Transform> waypoints;
    public Transform searchTarget;
    public float distanceToAnger = 3f;
    RaycastHit hit;
    
    void Start()
    {
        base.Start();
        pTimer = patrolWaitTime;
    }

    void Update()
    {
        if(isPatrol && !isRagdoll)
        {
            target = waypoints[curWaypoint];
            if(agent.remainingDistance <= 0.1f)
            {
                pTimer -= Time.deltaTime;
                if(pTimer <= 0)
                {
                    if(waypoints.Count == curWaypoint + 1)
                    {
                        curWaypoint = 0;
                        pTimer = patrolWaitTime;
                    }
                    else
                    {
                        curWaypoint += 1;
                        pTimer = patrolWaitTime;
                    }
                }
            }
            else
            {
                pTimer = patrolWaitTime;
            }
        }
        if(Vector3.Distance(transform.position, searchTarget.position) < distanceToAnger && Vector3.Dot(searchTarget.position - transform.position, transform.forward) > 0)
        {
            if(Physics.Raycast(transform.position, searchTarget.position - transform.position, out hit, distanceToAnger + 1f))
            {
                if(hit.transform == searchTarget)
                {
                    isPatrol = false;
                    target = searchTarget;
                }
            }
        }
        if(isPatrol && !isRagdoll) 
        {
            agent.destination = target.position; 
        }
        base.Update();
    }
}
