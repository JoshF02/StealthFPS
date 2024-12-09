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
        sm.Turret.color = Color.red;
        sm.Spotlight.color = Color.red;
        //Debug.Log("Combat state entered");
        sm.NmAgent.updateRotation = false;  // stops the nmAgent rotating the drone so it can be rotated manually
        shootTimer = 0f;
        sm.NmAgent.speed *= 2;

        if (sm.beenShot || sm.Hearing.AlertHeard == EnemyHearing.Alerts.EnterCombat) {
            Debug.Log("been shot or alerted so in combat for minimum 5 seconds");
            beenShotTimer = 0f;
        }
        else beenShotTimer = 5f;

        target = (!sm.Detection.GetDetectingPlayer() && sm.Detection.DetectingDecoy && !sm.beenShot) ? sm.Detection.Decoy : sm.Player;
        if (target == sm.Player) {
            sm.CombatAlertObj.SetActive(true);  // TARGET STUFF DOESNT WORK WITH ALERTS SO ONLY CREATE ALERT IF FIGHTING PLAYER
        }
        else Debug.Log("target is decoy so dont create alert");
    }

    public override void UpdateLogic()
    {
        if (target == sm.Detection.Decoy && sm.beenShot) {  
            Debug.Log("DECOY TARGET AND BEEN SHOT");
            beenShotTimer = 0f;
        }

        target = (!sm.Detection.GetDetectingPlayer() && sm.Detection.DetectingDecoy && !sm.beenShot) ? sm.Detection.Decoy : sm.Player;
        //Debug.Log("target is player: " + (target == sm.player));
        
        if (target != sm.Player && target == null) {
            Debug.Log("decoy dead so no longer detecting it, changing to hunt state");
            sm.Detection.StopDetectingDecoy(true);
            sm.ChangeState(sm.HuntState);
            return;
        }

        Vector3 dir = target.position - sm.transform.position;   // chases player but keeps a distance, still looks at player
        sm.NmAgent.destination = target.position - (4.0f * dir.normalized);  // 4 is min distance from player
        Quaternion rot = Quaternion.LookRotation(new(dir.x, 0, dir.z));
        sm.transform.rotation = Quaternion.Lerp(sm.transform.rotation, rot, 6.0f * Time.deltaTime); // 6 is turning speed

        if (!sm.Detection.GetDetectingTarget(target == sm.Player)) {   // transition if sight lost
            
            if (sm.Player.GetComponent<PlayerActions>().HasTeleported) {
                Debug.Log("teleport broke line of sight, going into hunt mode");
                sm.ChangeState(sm.HuntState);
                return;
            }

            if (beenShotTimer > 5f) {
                Debug.Log("sight lost, investigating last known position");
                sm.Detection.StartDetectingSuspicious(target);
                sm.ChangeState(sm.InvestigateState);
                sm.Detection.StopDetectingSuspicious();
                return;
            }
        }

        shootTimer += Time.deltaTime;   
        beenShotTimer += Time.deltaTime;
        sm.Turret.intensity = 0.3f + shootTimer;    // laser charges up
        sm.Spotlight.intensity = 200 + (shootTimer * 600);

        if (shootTimer > shootCooldown) {   // shoot at player when cooldown reached
            shootTimer = 0f;
            Debug.Log("shot at target " + target.tag);
            
            RaycastHit hit;
            if(Physics.Raycast(sm.transform.position + (dir.normalized * 1f), dir.normalized, out hit, 10.0f, sm.laserLayerMask) && hit.transform.tag == target.tag) {
                Debug.Log("target hit " + hit.transform.tag + " ---- " + hit.transform.name);
                if (target == sm.Player) GameManager.Instance.EndGame();
                else target.GetComponent<Target>().TakeDamage(10f); 
            }
        }

        base.UpdateLogic();
    }


    public override void Exit()
    {
        base.Exit();
        sm.Turret.intensity = 0.3f;
        sm.Spotlight.intensity = 200;
        sm.NmAgent.updateRotation = true;
        sm.NmAgent.speed /= 2;
        sm.CombatAlertObj.SetActive(false);

        sm.beenShot = false;
        sm.reenterCombatFromAlertCooldown = 1f;
    }
}
