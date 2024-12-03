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
        //Debug.Log("Patrol state entered");
        sm.detection.SetLessAware();
    }

    public override void Exit() // makes drone more aware when in a non-patrol state
    {
        base.Exit();
        sm.detection.SetMoreAware();
    }

    // contains looping through preset patrol points, pathfinding to them once each

    // transitions to hunt state if alerted by another nearby drone
}
