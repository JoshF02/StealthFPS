using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : NonCombatSuperstate
{
    public InvestigateState(DroneSM stateMachine) : base("InvestigateState", stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        Debug.Log("investigate state entered");
    }

    // contains pathfind to point of interest

    // transitions to hunt state after reaching it + waiting 2 seconds
}

