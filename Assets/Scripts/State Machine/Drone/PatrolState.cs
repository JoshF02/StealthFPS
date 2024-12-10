using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : MovingSuperstate
{
    public PatrolState(DroneSM stateMachine) : base("PatrolState", stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        sm.Turret.color = Color.green;
        sm.Spotlight.color = Color.green;
        //Debug.Log("Patrol state entered");
        sm.Detection.SetLessAware();

        sm.NmAgent.destination = sm.Waypoints[sm.PatrolIndex];
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.Hearing.AlertHeard == EnemyHearing.Alerts.EnterHunt)
        {
            Debug.Log("Enter hunt alert recieved");
            sm.ChangeState(sm.HuntState);
            return;
        }

        if ((Mathf.Abs(sm.transform.position.y - sm.Waypoints[sm.PatrolIndex].y) < 2.5f) && // if within height limit
            Vector2.Distance(new(sm.transform.position.x, sm.transform.position.z), new(sm.Waypoints[sm.PatrolIndex].x, sm.Waypoints[sm.PatrolIndex].z)) < 0.5f)    // if coordinates reached
        { 
            sm.PatrolIndex = (sm.PatrolIndex + 1) % sm.Waypoints.Length;    // get next patrol point and pathfind to it
            sm.NmAgent.destination = sm.Waypoints[sm.PatrolIndex];
        }
    }

    public override void Exit() // makes drone more aware when in a non-patrol state
    {
        base.Exit();
        sm.Detection.SetMoreAware();
    }
}
