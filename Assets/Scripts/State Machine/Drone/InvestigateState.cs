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

        //sm.nmAgent.destination = sm.detection.suspicousObject.position; // pathfind to point of interest

        if (sm.hearing.GetNoiseHeard()) sm.nmAgent.destination = sm.hearing.noisePos;
        else sm.nmAgent.destination = sm.detection.suspicousObject.position;



        /*if (sm.detection.suspicousObject.tag == "Player") {
            sm.detection.suspicousObject = GameObject.Instantiate(sm.detection.suspicousObject.GetChild(2), sm.detection.suspicousObject.position, Quaternion.identity);
        }*/
        
        timer = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        //Debug.Log(sm.nmAgent.destination + " --------- " + sm.detection.suspicousObject.position);

        // once current player position has been targeted by pathfinding, set suspicious object to null so it goes to that position instead of continuously pathfinding to player
        /*if (sm.detection.suspicousObject != null && sm.detection.suspicousObject.tag == "Player" 
        && sm.nmAgent.destination.x == sm.detection.suspicousObject.position.x && sm.nmAgent.destination.z == sm.detection.suspicousObject.position.z) {
            Debug.Log("setting to null");
            sm.detection.suspicousObject = null;
        }*/

        if (!sm.hearing.GetNoiseHeard() && sm.detection.suspicousObject != null && Vector3.Distance(sm.nmAgent.destination, sm.detection.suspicousObject.position) > 2.0f) {   // if suspicious object moves/updated
            sm.nmAgent.destination = sm.detection.suspicousObject.position;
        }
        else if (sm.hearing.GetNoiseHeard()) sm.nmAgent.destination = sm.hearing.noisePos;

        if (Vector3.Distance(sm.nmAgent.destination, sm.transform.position) < 3.0f) {   // transitions to hunt state after reaching it + waiting 2 seconds
            timer += Time.deltaTime;

            if (timer > 2.0f) {
                timer = 0f;
                Debug.Log("finished investigating, changing to hunt state");

                if (sm.hearing.GetNoiseHeard()) {
                    sm.hearing.SetNoiseHeard(false);
                    sm.hearing.noisePos = new();
                    sm.ChangeState(sm.huntState);
                    return;
                }

                if (sm.detection.suspicousObject != null) {
                    if (sm.detection.suspicousObject.tag == "DeleteOnInvestigate") GameObject.Destroy(sm.detection.suspicousObject.gameObject);
                    else sm.detection.suspicousObject.tag = "Investigated";
                    sm.detection.suspicousObject = null;
                }
                sm.detection.SetDetectingSuspicious(false);

                sm.ChangeState(sm.huntState);
            }
        }
    }
}

