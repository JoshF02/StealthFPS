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
        sm.turret.intensity = 0;
        sm.spotlight.intensity = 0;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        sm.turret.intensity = 0;
        sm.spotlight.intensity = 0;

        sm.hearing.disableForSecs -= Time.deltaTime;
        //Debug.Log("in disabled state for " + sm.hearing.disableForSecs);

        if (sm.hearing.disableForSecs <= 0) {
            Debug.Log("exiting disabled state, into hunt");
            sm.ChangeState(sm.huntState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        sm.turret.intensity = 0.3f;
        sm.spotlight.intensity = 200;
    }
}
