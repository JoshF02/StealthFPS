using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntState : MovingSuperstate
{
    public HuntState(DroneSM stateMachine) : base("HuntState", stateMachine) {}

    private float timer = 0f;

    public override void Enter()
    {
        base.Enter();
        sm.turret.color = Color.yellow;
        sm.spotlight.color = Color.yellow;
        //Debug.Log("hunt state entered");
        sm.nmAgent.destination = sm.transform.position;
        sm.nmAgent.speed *= 2;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if ((Mathf.Abs(sm.transform.position.y - sm.nmAgent.destination.y) < 2.5f) && // if within height limit
                                Vector2.Distance(new Vector2(sm.transform.position.x, sm.transform.position.z), 
                                new Vector2(sm.nmAgent.destination.x, sm.nmAgent.destination.z)) < 0.5f) { // if coordinates reached

            Vector3 dir = new Vector3(((Random.value * 2) - 1), 0, ((Random.value * 2) - 1)); // picks random direction to raycast in

            RaycastHit hit;
            if(Physics.Raycast(sm.transform.position + (dir * 0.7f), dir, out hit, 10.0f)) {
                //Debug.Log("RAYCAST HIT " + hit.collider.gameObject.name);
                if (hit.distance > 5.0f) sm.nmAgent.destination = hit.point;// - (dir * 2.0f);  // if hit point isnt too close then go there
                                                                                                // else do nothing so it tries again next update
            }
            else {
                //Debug.Log("RAYCAST MISS");
                sm.nmAgent.destination = sm.transform.position + (dir * 10.0f); // if no hit then go to empty space
            }

            sm.testingSphere.position = sm.nmAgent.destination;
        }

        timer += Time.deltaTime;

        if (timer > 15.0f) { // transitions to patrol after x time passed
            timer = 0f;
            Debug.Log("finished hunting, changing to patrol state");
            sm.ChangeState(sm.patrolState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        sm.nmAgent.speed /= 2;
    }

    // alert any other drones that enter large radius around it (alarm noise)
}
