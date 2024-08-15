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
}
