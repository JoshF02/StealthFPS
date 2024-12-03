using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : NonCombatSuperstate
{
    public InvestigateState(DroneSM stateMachine) : base("InvestigateState", stateMachine) {}

    private float timer = 0f;

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("investigate state entered");

        sm.nmAgent.destination = sm.detection.suspicousObject.position; // pathfind to point of interest
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (sm.detection.suspicousObject != null && Vector3.Distance(sm.nmAgent.destination, sm.detection.suspicousObject.position) > 2.0f) {   // if suspicious object moves
            sm.nmAgent.destination = sm.detection.suspicousObject.position;
        }

        if (Vector3.Distance(sm.nmAgent.destination, sm.transform.position) < 3.0f) {   // transitions to hunt state after reaching it + waiting 2 seconds
            timer += Time.deltaTime;

            if (timer > 2.0f) {
                timer = 0f;
                Debug.Log("finished investigating, changing to hunt state");

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

