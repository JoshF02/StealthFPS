using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHearing : MonoBehaviour
{
    NavMeshAgent agent;
    NavMeshPath path;
    private bool noiseHeard = false;
    [HideInInspector] public Vector3 noisePos = new();

    public bool GetNoiseHeard()
    {
        return noiseHeard;
    }

    public void SetNoiseHeard(bool val)
    {
        noiseHeard = val;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    void OnTriggerStay(Collider other)  // goes into investigate state to check out noise if it can hear it
    {
        noiseHeard = false; 

        if (other.tag == "Sound" && agent.CalculatePath(other.transform.position, path) && (path.status == NavMeshPathStatus.PathComplete)) {  
            float length = CalculatePathLength(path);
            float straightLineDist = Vector3.Distance(transform.position, other.transform.position);

            if (length < (straightLineDist * 2)) {      // if path less than multiple of straight line distance (PLAY AROUND WITH THE MULTIPLIER)
                noiseHeard = true;
                noisePos = other.transform.parent.position;
                //Debug.Log("valid noise heard");
            }
            else Debug.Log("path too long, can't hear noise");  // does these calculations every frame, could optimise

            // USING HUMANOID NAVMESH SO TINY GAPS WILL OBSTRUCT - KEEP IN MIND
        }
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
