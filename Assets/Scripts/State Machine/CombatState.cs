using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : BaseState
{
    protected DroneSM sm;

    public CombatState(DroneSM stateMachine) : base("CombatState", stateMachine)
    {
        sm = (DroneSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Combat state entered");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        sm.nmAgent.destination = sm.player.position;

        if (!sm.detection.GetDetectingPlayer()) {   // transition to hunt state if sight lost
            Debug.Log("sight lost, changing to hunt state");
            sm.ChangeState(sm.huntState);
        }
    }

    // contains laser charge up 2-3 secs then shoot at player, instantly kill?
}
