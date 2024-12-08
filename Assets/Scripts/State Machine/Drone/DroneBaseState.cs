using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBaseState : BaseState
{
    protected DroneSM sm;

    public DroneBaseState(string name, DroneSM stateMachine) : base(name, stateMachine)
    {
        sm = (DroneSM)this.stateMachine;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.hearing.disableForSecs > 0) {
            Debug.Log("disabling for " + sm.hearing.disableForSecs);
            sm.ChangeState(sm.disabledState);
        }
    }
}
