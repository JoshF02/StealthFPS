using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : BaseState
{
    protected DroneSM sm;
    private float shootTimer = 0f;
    private readonly float shootCooldown = 4.0f;
    private float beenShotTimer = 0f;

    public CombatState(DroneSM stateMachine) : base("CombatState", stateMachine)
    {
        sm = (DroneSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        sm.turret.color = Color.red;
        sm.spotlight.color = Color.red;
        //Debug.Log("Combat state entered");
        sm.nmAgent.updateRotation = false;  // stops the nmAgent rotating the drone so it can be rotated manually
        shootTimer = 0f;
        sm.nmAgent.speed *= 2;

        if (sm.beenShot) {
            Debug.Log("been shot so in combat for minimum 5 seconds");
            beenShotTimer = 0f;
        }
        else beenShotTimer = 5f;

        sm.beenShot = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Vector3 dir = sm.player.position - sm.transform.position;   // chases player but keeps a distance, still looks at player
        sm.nmAgent.destination = sm.player.position - (3.0f * dir.normalized);  // 3 is min distance from player
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        sm.transform.rotation = Quaternion.Lerp(sm.transform.rotation, rot, 6.0f * Time.deltaTime); // 6 is turning speed

        if (!sm.detection.GetDetectingPlayer(sm.transform.position, sm.player.position) && beenShotTimer > 5f) {   // transition to investigate state if sight lost
            Debug.Log("sight lost, investigating last known position");
            sm.detection.suspicousObject = sm.player;
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
            Debug.Log("shot at player");
            
            RaycastHit hit;
            if(Physics.Raycast(sm.transform.position, dir, out hit, 10.0f, sm.laserLayerMask)) {
                Debug.Log("player hit, game over");
                GameManager.Instance.EndGame();
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
    }
}
