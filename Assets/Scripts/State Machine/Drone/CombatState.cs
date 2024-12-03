using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : BaseState
{
    protected DroneSM sm;
    private float shootTimer = 0f;
    private readonly float shootCooldown = 3.0f;

    public CombatState(DroneSM stateMachine) : base("CombatState", stateMachine)
    {
        sm = (DroneSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        sm.turret.color = Color.red;
        //Debug.Log("Combat state entered");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Vector3 dir = sm.player.position - sm.nmAgent.transform.position;   // chases player but keeps a distance, still looks at player
        sm.nmAgent.destination = sm.player.position - (3.0f * dir.normalized);  // 3 is min distance from player
        sm.nmAgent.angularSpeed = 0;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        sm.nmAgent.transform.rotation = Quaternion.Lerp(sm.nmAgent.transform.rotation, rot, 6.0f * Time.deltaTime); // 6 is turning speed

        if (!sm.detection.GetDetectingPlayer()) {   // transition to investigate state if sight lost
            Debug.Log("sight lost, investigating last known position");
            sm.detection.suspicousObject = sm.player;
            sm.detection.SetDetectingSuspicious(true);
            sm.ChangeState(sm.investigateState);
            sm.detection.suspicousObject = null;
            return;
        }

        shootTimer += Time.deltaTime;   
        sm.turret.intensity = Mathf.Lerp(0.3f, 2.0f, shootTimer / shootCooldown);

        if (shootTimer > shootCooldown) {   // shoot at player when cooldown reached
            shootTimer = 0f;
            Debug.Log("shot at player");
            
            RaycastHit hit;
            if(Physics.Raycast(sm.nmAgent.transform.position, dir, out hit, 10.0f, sm.laserLayerMask)) {
                Debug.Log("player hit, game over");
            }
        }
    }


    public override void Exit()
    {
        base.Exit();
        sm.turret.intensity = 0.3f;
        //sm.turret.color = Color.yellow;
    }

    // contains laser charge up 2-3 secs then shoot at player, instantly kill?
}
