using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : MovingSuperstate
{
    public PatrolState(DroneSM stateMachine) : base("PatrolState", stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Patrol state entered");
    }

    // contains looping through preset patrol points, pathfinding to them once each

    // transitions to hunt state if alerted by another nearby drone
}
