using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledState : BaseState
{
    protected DroneSM sm;

    public DisabledState(DroneSM stateMachine) : base("DisabledState", stateMachine)
    {
        sm = (DroneSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        sm.Turret.intensity = 0;
        sm.Spotlight.intensity = 0;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.Hearing.DisableForSecs <= 0) {
            Debug.Log("exiting disabled state, into hunt");
            sm.ChangeState(sm.HuntState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        sm.Turret.intensity = 0.3f;
        sm.Spotlight.intensity = 200;
    }
}
