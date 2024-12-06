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
        sm.turret.color = new Color(1, 0.64f, 0, 1);
        sm.spotlight.color = new Color(1, 0.64f, 0, 1);
        //Debug.Log("investigate state entered");

        if (sm.hearing.GetNoiseHeard()) sm.nmAgent.destination = sm.hearing.noisePos;
        else sm.nmAgent.destination = sm.detection.suspicousObject.position;
        
        timer = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.hearing.GetNoiseHeard()) sm.nmAgent.destination = sm.hearing.noisePos;
        else if (sm.detection.suspicousObject != null) sm.nmAgent.destination = sm.detection.suspicousObject.position;

        if (Vector3.Distance(sm.nmAgent.destination, sm.transform.position) < 3.0f) {   // transitions to hunt state after reaching it + waiting 2 seconds
            timer += Time.deltaTime;

            if (timer > 2.0f) {
                timer = 0f;
                Debug.Log("finished investigating, changing to hunt state");

                if (sm.hearing.GetNoiseHeard()) {
                    sm.hearing.SetNoiseHeard(false);
                    sm.ChangeState(sm.huntState);
                    return;
                }

                if (sm.detection.suspicousObject != null) {
                    sm.detection.suspicousObject.tag = "Investigated";
                    sm.detection.suspicousObject = null;
                }
                sm.detection.SetDetectingSuspicious(false);

                sm.ChangeState(sm.huntState);
            }
        }
    }
}

