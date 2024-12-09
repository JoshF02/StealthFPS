using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : DroneBaseState
{
    private float shootTimer = 0f;
    private readonly float shootCooldown = 4.0f;
    private float beenShotTimer = 0f;
    private Transform target = null;

    public CombatState(DroneSM stateMachine) : base("CombatState", stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        sm.turret.color = Color.red;
        sm.spotlight.color = Color.red;
        //Debug.Log("Combat state entered");
        sm.nmAgent.updateRotation = false;  // stops the nmAgent rotating the drone so it can be rotated manually
        shootTimer = 0f;
        sm.nmAgent.speed *= 2;

        if (sm.beenShot || sm.hearing.GetAlertHeard() == EnemyHearing.Alerts.EnterCombat) {
            Debug.Log("been shot or alerted so in combat for minimum 5 seconds");
            beenShotTimer = 0f;
        }
        else beenShotTimer = 5f;

        //sm.beenShot = false;

        target = (!sm.detection.GetDetectingPlayer(sm.transform.position, sm.player.position) && sm.detection.GetDetectingDecoy() && !sm.beenShot) ? sm.detection.decoy : sm.player;
        if (target == sm.player) {
            sm.combatAlertObj.SetActive(true);  // TARGET STUFF DOESNT WORK WITH ALERTS SO ONLY CREATE ALERT IF FIGHTING PLAYER
        }
        else Debug.Log("target is decoy so dont create alert");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (target == sm.detection.decoy && sm.beenShot) {  
            Debug.Log("DECOY TARGET AND BEEN SHOT");
            beenShotTimer = 0f;
        }

        target = (!sm.detection.GetDetectingPlayer(sm.transform.position, sm.player.position) && sm.detection.GetDetectingDecoy() && !sm.beenShot) ? sm.detection.decoy : sm.player;
        //Debug.Log("target is player: " + (target == sm.player));
        
        if (target != sm.player && target == null) {
            Debug.Log("decoy dead so no longer detecting it, changing to hunt state");
            sm.detection.SetDetectingDecoy(false);
            sm.ChangeState(sm.huntState);
            return;
        }

        Vector3 dir = target.position - sm.transform.position;   // chases player but keeps a distance, still looks at player
        sm.nmAgent.destination = target.position - (4.0f * dir.normalized);  // 4 is min distance from player
        Quaternion rot = Quaternion.LookRotation(new(dir.x, 0, dir.z));
        sm.transform.rotation = Quaternion.Lerp(sm.transform.rotation, rot, 6.0f * Time.deltaTime); // 6 is turning speed

        if (!sm.detection.GetDetectingTarget(sm.transform.position, target.position, (target == sm.player)) && beenShotTimer > 5f) {   // transition to investigate state if sight lost
            Debug.Log("sight lost, investigating last known position");
            sm.detection.suspicousObject = target;
            sm.detection.SetDetectingSuspicious(true);
            sm.ChangeState(sm.investigateState);
            sm.detection.suspicousObject = null;
            return;
        }

        shootTimer += Time.deltaTime;   
        beenShotTimer += Time.deltaTime;
        sm.turret.intensity = 0.3f + shootTimer;    // laser charges up
        sm.spotlight.intensity = 200 + (shootTimer * 600);

        if (shootTimer > shootCooldown) {   // shoot at player when cooldown reached
            shootTimer = 0f;
            Debug.Log("shot at target " + target.tag);
            
            RaycastHit hit;
            if(Physics.Raycast(sm.transform.position + (dir.normalized * 1f), dir.normalized, out hit, 10.0f, sm.laserLayerMask) && hit.transform.tag == target.tag) {
                Debug.Log("target hit " + hit.transform.tag + " ---- " + hit.transform.name);
                if (target == sm.player) GameManager.Instance.EndGame();
                else target.GetComponent<Target>().TakeDamage(10f); 
                /*{
                    Target targetComp = target.GetComponent<Target>();
                    if (targetComp.health <= 10) {
                        Debug.Log("killing decoy so no longer detecting it, changing to hunt state");
                        sm.detection.SetDetectingDecoy(false);
                        sm.ChangeState(sm.huntState);
                    }
                    targetComp.TakeDamage(10f);
                }*/
            }
        }
    }


    public override void Exit()
    {
        base.Exit();
        sm.turret.intensity = 0.3f;
        sm.spotlight.intensity = 200;
        sm.nmAgent.updateRotation = true;
        sm.nmAgent.speed /= 2;
        sm.combatAlertObj.SetActive(false);

        sm.beenShot = false;
    }
}
