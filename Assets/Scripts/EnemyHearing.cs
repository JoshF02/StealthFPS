using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHearing : MonoBehaviour
{
    NavMeshAgent agent;
    NavMeshPath path;
    public enum Alerts
    {
        None,
        EnterHunt,
        EnterCombat,
        Max
    }
    public bool NoiseHeard { get; private set; } = false;
    public Alerts AlertHeard { get; private set; } = Alerts.None;
    public float DisableForSecs { get; private set; } = 0;
    public Vector3 NoisePos { get; private set; } = new();

    public void StopHearingNoise()
    {
        NoiseHeard = false;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    void Update()
    {
        DisableForSecs -= Time.deltaTime;
    }

    void OnTriggerStay(Collider other)  // goes into investigate state to check out noise if it can hear it
    {
        NoiseHeard = false; 

        if (other.tag == "Sound" && agent.CalculatePath(other.transform.position, path) && (path.status == NavMeshPathStatus.PathComplete)) {  
            float length = CalculatePathLength(path);
            float straightLineDist = Vector3.Distance(transform.position, other.transform.position);

            if (length < (straightLineDist * 2)) {      // if path less than multiple of straight line distance (PLAY AROUND WITH THE MULTIPLIER)
                NoiseHeard = true;
                NoisePos = other.transform.parent.position;
                //Debug.Log("valid noise heard");
            }
            //else Debug.Log("path too long, can't hear noise");  // does these calculations every frame, could optimise

            // USING HUMANOID NAVMESH SO TINY GAPS WILL OBSTRUCT - KEEP IN MIND
        }

        if (other.tag == "Alert") {
            AlertHeard = (other.name == "EnterHuntAlert") ? Alerts.EnterHunt : Alerts.EnterCombat;  // NOTE - DOESNT DO PATHFINDING FOR ALERTS (could do it?)
            //Debug.Log(alertHeard + " alert heard by " + transform.parent.gameObject.name);
        }

        if (other.tag == "EMP") {
            //Debug.Log("emp detected");
            DisableForSecs = 10f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("ON TRIGGER EXIT CALLED");
        NoiseHeard = false;
        AlertHeard = Alerts.None;
    }

    public float CalculatePathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2) return 0;

        float lengthSoFar = 0.0f;
        Vector3 previousCorner = path.corners[0];

        for (int i = 1; i < path.corners.Length; i++) {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
        }

        return lengthSoFar;
    }
}
