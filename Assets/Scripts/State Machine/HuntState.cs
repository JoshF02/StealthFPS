using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntState : MovingSuperstate
{
    public HuntState(DroneSM stateMachine) : base("HuntState", stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        Debug.Log("hunt state entered");
    }

    // contains pathfind to random? points in larger area, faster movement, alert any other drones that enter large radius around it

    // transitions to patrol after x time passed
}
