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
}
