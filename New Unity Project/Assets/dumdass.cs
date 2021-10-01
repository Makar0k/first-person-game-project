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
    }

    void Update()
    {
        if(isPatrol)
        {
            target = waypoints[curWaypoint];
            print(curWaypoint);
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


        Debug.DrawRay(transform.position, searchTarget.position - transform.position, Color.red);
        if(Vector3.Distance(transform.position, searchTarget.position) < distanceToAnger && Vector3.Dot(searchTarget.position - transform.position, transform.forward) > 0)
        {
            if(Physics.Raycast(transform.position, searchTarget.position - transform.position, out hit, distanceToAnger + 1f))
            {
                print(hit.transform);
                if(hit.transform == searchTarget)
                {
                    isPatrol = false;
                    target = searchTarget;
                }
            }
        }
        base.Update();
    }
}
