using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonCombatSuperstate : BaseState
{
    private DroneSM sm;

    public NonCombatSuperstate(string name, DroneSM stateMachine) : base(name, stateMachine)
    {
        sm = (DroneSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Non combat superstate entered");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.detection.GetDetectingPlayer()) {    // transition to combat if player detected visually
            Debug.Log("changing to combat state");
            sm.ChangeState(sm.combatState);
        }
    }
}
