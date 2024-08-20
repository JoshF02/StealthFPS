using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSuperstate : NonCombatSuperstate
{
    public MovingSuperstate(string name, DroneSM stateMachine) : base(name, stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("moving superstate entered");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.detection.GetDetectingSuspicious()) {    // transition to investigate if suspicious object detected visually (add audio later)
            Debug.Log("suspicous object spotted, changing to investigate state");
            sm.ChangeState(sm.investigateState);
        }
    }
}
