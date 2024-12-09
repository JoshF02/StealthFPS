using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateState : NonCombatSuperstate
{
    public InvestigateState(DroneSM stateMachine) : base("InvestigateState", stateMachine) {}

    private float timer = 0f;

    public override void Enter()
    {
        base.Enter();
        sm.Turret.color = new Color(1, 0.64f, 0, 1);
        sm.Spotlight.color = new Color(1, 0.64f, 0, 1);
        //Debug.Log("investigate state entered");

        if (sm.Hearing.NoiseHeard) sm.NmAgent.destination = sm.Hearing.NoisePos;
        else if (sm.Detection.SuspicousObject != null) sm.NmAgent.destination = sm.Detection.SuspicousObject.position;
        
        timer = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.Hearing.NoiseHeard) sm.NmAgent.destination = sm.Hearing.NoisePos;
        else if (sm.Detection.SuspicousObject != null) sm.NmAgent.destination = sm.Detection.SuspicousObject.position;

        if (Vector3.Distance(sm.NmAgent.destination, sm.transform.position) < 3.0f) {   // transitions to hunt state after reaching it + waiting 2 seconds
            timer += Time.deltaTime;

            if (timer > 2.0f) {
                timer = 0f;
                Debug.Log("finished investigating, changing to hunt state");

                if (sm.Hearing.NoiseHeard) {
                    sm.Hearing.StopHearingNoise();
                    sm.ChangeState(sm.HuntState);
                    return;
                }

                if (sm.Detection.SuspicousObject != null) sm.Detection.SuspicousObject.tag = "Investigated";
                sm.Detection.StopDetectingSuspicious(true);

                sm.ChangeState(sm.HuntState);
            }
        }
    }
}

