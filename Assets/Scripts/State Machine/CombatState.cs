using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : BaseState
{
    private DroneSM sm;

    public CombatState(DroneSM stateMachine) : base("CombatState", stateMachine)
    {
        sm = (DroneSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Combat state entered");
    }
}
