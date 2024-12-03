using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntState : MovingSuperstate
{
    public HuntState(DroneSM stateMachine) : base("HuntState", stateMachine) {}

    private float timer = 0f;

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("hunt state entered");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        timer += Time.deltaTime;

        if (timer > 5.0f) { // transitions to patrol after x time passed
            timer = 0f;
            Debug.Log("finished hunting, changing to patrol state");
            sm.ChangeState(sm.patrolState);
        }
    }

    // contains pathfind to random? points in larger area, faster movement, alert any other drones that enter large radius around it
}
