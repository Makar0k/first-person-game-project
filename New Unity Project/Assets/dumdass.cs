using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumdass : enemyAI, INavLinkHandler
{
    public bool isPatrol;
    public float patrolWaitTime;
    byte curWaypoint = 0;
    float pTimer;
    public List<Transform> waypoints;
    public Transform searchTarget;
    public float distanceToAnger = 3f;
    public AgentLinkMover agentMover;
    protected float LinkEndTimerValue;
    RaycastHit hit;
    
    new void Start()
    {
        agentMover = GetComponent<AgentLinkMover>();
        base.Start();
        pTimer = patrolWaitTime;
        agentMover.StartMove += LinkHandleStart;
        agentMover.EndMove += LinkHandleEnd; 
    }
    public void LinkHandleStart()
    {
        ChangeMovementSpeed(0);
    }
    public void LinkHandleEnd()
    {
        StartCoroutine(StartLinkEndTimer(3f));
    }

    // Some StackOverflow Courotine Timer
    public IEnumerator StartLinkEndTimer(float timerValue)
    {
        LinkEndTimerValue = timerValue;
        while (LinkEndTimerValue > 0)
        {
            Debug.Log("Timer: " + LinkEndTimerValue);
            yield return new WaitForSeconds(1.0f);
            LinkEndTimerValue--;
        }
        ChangeMovementSpeed(speed);
    }

    new void Update()
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
