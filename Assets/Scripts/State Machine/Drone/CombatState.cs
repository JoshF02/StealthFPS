using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : BaseState
{
    protected DroneSM sm;
    private float shootTimer = 0f;
    private readonly float shootCooldown = 3.0f;
    private Color turretColor = new Color(1, 0.64f, 0, 1);

    public CombatState(DroneSM stateMachine) : base("CombatState", stateMachine)
    {
        sm = (DroneSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Combat state entered");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //sm.nmAgent.destination = sm.player.position;

        // chases player but keeps a distance, still looks at player
        Vector3 dir = sm.player.position - sm.nmAgent.transform.position;
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
        }

        shootTimer += Time.deltaTime;   
        turretColor.g = Mathf.Lerp(0.64f, 0, (shootTimer / shootCooldown)); // turret light darkens from orange to red while charging shot
        sm.turret.color = turretColor;

        if (shootTimer > shootCooldown) {   // shoot at player when cooldown reached
            shootTimer = 0f;
            Debug.Log("shot at player");
        }
    }

    // contains laser charge up 2-3 secs then shoot at player, instantly kill?
}
