using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonCombatSuperstate : DroneBaseState
{
    public NonCombatSuperstate(string name, DroneSM stateMachine) : base(name, stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Non combat superstate entered");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.Detection.GetDetectingPlayer() || sm.BeenShot)   // transition to combat if player detected visually, or shot by player
        {    
            Debug.Log("player spotted, changing to combat state");
            sm.ChangeState(sm.CombatState);
            return;
        }

        sm.ReenterCombatFromAlertCooldown -= Time.deltaTime;
        if (sm.Hearing.AlertHeard == EnemyHearing.Alerts.EnterCombat && sm.ReenterCombatFromAlertCooldown <= 0f)    // put in if statement above after done testing
        {    
            Debug.Log("Enter combat alert recieved");
            sm.ChangeState(sm.CombatState);
            return;
        }

        if (sm.Detection.DetectingDecoy)
        {
            Debug.Log("decoy detected, entering combat state");
            sm.ChangeState(sm.CombatState);
            return;
        }
    }
}
