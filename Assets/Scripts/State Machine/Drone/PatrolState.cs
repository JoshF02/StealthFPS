using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : MovingSuperstate
{
    public PatrolState(DroneSM stateMachine) : base("PatrolState", stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        sm.turret.color = Color.green;
        sm.spotlight.color = Color.green;
        //Debug.Log("Patrol state entered");
        sm.detection.SetLessAware();

        sm.nmAgent.destination = sm.waypoints[sm.patrolIndex];
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.hearing.GetAlertHeard() == EnemyHearing.Alerts.EnterHunt) {
            Debug.Log("Enter hunt alert recieved");
            sm.ChangeState(sm.huntState);
            return;
        }

        if ((Mathf.Abs(sm.transform.position.y - sm.waypoints[sm.patrolIndex].y) < 2.5f) && // if within height limit
                                Vector2.Distance(new Vector2(sm.transform.position.x, sm.transform.position.z), 
                                new Vector2(sm.waypoints[sm.patrolIndex].x, sm.waypoints[sm.patrolIndex].z)) < 0.5f) { // if coordinates reached

            sm.patrolIndex = (sm.patrolIndex + 1) % sm.waypoints.Length;    // get next patrol point and pathfind to it
            sm.nmAgent.destination = sm.waypoints[sm.patrolIndex];
        }
    }

    public override void Exit() // makes drone more aware when in a non-patrol state
    {
        base.Exit();
        sm.detection.SetMoreAware();
    }
}
