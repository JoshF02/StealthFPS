using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntState : MovingSuperstate
{
    public HuntState(DroneSM stateMachine) : base("HuntState", stateMachine) {}

    private float _timer = 0f;

    public override void Enter()
    {
        base.Enter();
        sm.Turret.color = Color.yellow;
        sm.Spotlight.color = Color.yellow;
        //Debug.Log("hunt state entered");
        sm.NmAgent.destination = sm.transform.position;
        sm.NmAgent.speed *= 2;
        _timer = 0f;
        sm.HuntAlertObj.SetActive(true);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if ((Mathf.Abs(sm.transform.position.y - sm.NmAgent.destination.y) < 2.5f) && // if within height limit
            Vector2.Distance(new(sm.transform.position.x, sm.transform.position.z), new(sm.NmAgent.destination.x, sm.NmAgent.destination.z)) < 0.5f)    // if coordinates reached
        { 
            Vector3 dir = new Vector3(((Random.value * 2) - 1), 0, ((Random.value * 2) - 1)); // picks random direction to raycast in

            if (Physics.Raycast(sm.transform.position + (dir * 0.7f), dir, out RaycastHit hit, 10.0f))
            {
                //Debug.Log("RAYCAST HIT " + hit.collider.gameObject.name);
                if (hit.distance > 5.0f) sm.NmAgent.destination = hit.point;// - (dir * 2.0f);  // if hit point isnt too close then go there
                                                                            // else do nothing so it tries again next update
            }
            else
            {
                //Debug.Log("RAYCAST MISS");
                sm.NmAgent.destination = sm.transform.position + (dir * 10.0f); // if no hit then go to empty space
            }
        }

        _timer += Time.deltaTime;

        if (_timer > 15.0f) // transitions to patrol after x time passed
        { 
            _timer = 0f;
            Debug.Log("finished hunting, changing to patrol state");
            sm.ChangeState(sm.PatrolState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        sm.NmAgent.speed /= 2;
        sm.HuntAlertObj.SetActive(false);
    }
}
