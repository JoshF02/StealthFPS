using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSuperstate : NonCombatSuperstate
{
    public MovingSuperstate(string name, DroneSM stateMachine) : base(name, stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        Debug.Log("moving superstate entered");
    }

    // contains transition to investigate if suspicious object detected visually/audially
}
