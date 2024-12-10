using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : DroneBaseState
{
    private float _shootTimer = 0f;
    private readonly float _shootCooldown = 4.0f;
    private float _beenShotTimer = 0f;
    private Transform _target = null;

    public CombatState(DroneSM stateMachine) : base("CombatState", stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        sm.Turret.color = Color.red;
        sm.Spotlight.color = Color.red;
        //Debug.Log("Combat state entered");
        sm.NmAgent.updateRotation = false;  // stops the nmAgent rotating the drone so it can be rotated manually
        _shootTimer = 0f;
        sm.NmAgent.speed *= 2;

        if (sm.BeenShot || (sm.Hearing.AlertHeard == EnemyHearing.Alerts.EnterCombat))
        {
            Debug.Log("been shot or alerted so in combat for minimum 5 seconds");
            _beenShotTimer = 0f;
        }
        else _beenShotTimer = 5f;

        _target = (!sm.Detection.GetDetectingPlayer() && sm.Detection.DetectingDecoy && !sm.BeenShot) ? sm.Detection.Decoy : sm.Player;

        if (_target == sm.Player) sm.CombatAlertObj.SetActive(true);  // TARGET STUFF DOESNT WORK WITH ALERTS SO ONLY CREATE ALERT IF FIGHTING PLAYER
        else Debug.Log("target is decoy so dont create alert");
    }

    public override void UpdateLogic()
    {
        if ((_target == sm.Detection.Decoy) && sm.BeenShot)
        {  
            Debug.Log("DECOY TARGET AND BEEN SHOT");
            _beenShotTimer = 0f;
        }

        _target = (!sm.Detection.GetDetectingPlayer() && sm.Detection.DetectingDecoy && !sm.BeenShot) ? sm.Detection.Decoy : sm.Player;
        //Debug.Log("target is player: " + (target == sm.player));
        
        if ((_target != sm.Player) && (_target == null))
        {
            Debug.Log("decoy dead so no longer detecting it, changing to hunt state");
            sm.Detection.StopDetectingDecoy(true);
            sm.ChangeState(sm.HuntState);
            return;
        }

        Vector3 dir = _target.position - sm.transform.position;   // chases player but keeps a distance, still looks at player
        sm.NmAgent.destination = _target.position - (4.0f * dir.normalized);  // 4 is min distance from player
        Quaternion rot = Quaternion.LookRotation(new(dir.x, 0, dir.z));
        sm.transform.rotation = Quaternion.Lerp(sm.transform.rotation, rot, 6.0f * Time.deltaTime); // 6 is turning speed

        if (!sm.Detection.GetDetectingTarget(_target == sm.Player)) // transition if sight lost
        {   
            
            if (sm.Player.GetComponent<PlayerActions>().HasTeleported)
            {
                Debug.Log("teleport broke line of sight, going into hunt mode");
                sm.ChangeState(sm.HuntState);
                return;
            }

            if (_beenShotTimer > 5f)
            {
                Debug.Log("sight lost, investigating last known position");
                sm.Detection.StartDetectingSuspicious(_target);
                sm.ChangeState(sm.InvestigateState);
                sm.Detection.StopDetectingSuspicious();
                return;
            }
        }

        _shootTimer += Time.deltaTime;   
        _beenShotTimer += Time.deltaTime;
        sm.Turret.intensity = 0.3f + _shootTimer;    // laser charges up
        sm.Spotlight.intensity = 200 + (_shootTimer * 600);

        if (_shootTimer > _shootCooldown)   // shoot at player when cooldown reached
        {   
            _shootTimer = 0f;
            Debug.Log("shot at target " + _target.tag);

            if (Physics.Raycast(sm.transform.position + (dir.normalized * 1f), dir.normalized, out RaycastHit hit, 10.0f, sm.LaserLayerMask) && hit.transform.tag == _target.tag)
            {
                Debug.Log("target hit " + hit.transform.tag + " ---- " + hit.transform.name);
                if (_target == sm.Player) GameManager.Instance.EndGame();
                else _target.GetComponent<Target>().TakeDamage(10f);
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

        sm.BeenShot = false;
        sm.ReenterCombatFromAlertCooldown = 1f;
    }
}
