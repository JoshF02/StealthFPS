using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHearing : MonoBehaviour
{
    //EnemyDetection detection;
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
        //detection = transform.GetChild(2).GetComponent<EnemyDetection>();
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    void OnTriggerStay(Collider other)  // goes into investigate state to check out noise if it can hear it
    {
        noiseHeard = false; 

        if (other.tag == "Sound" /*&& !detection.GetDetectingSuspicious()*/ && agent.CalculatePath(other.transform.position, path) && (path.status == NavMeshPathStatus.PathComplete)) {  

            float length = CalculatePathLength(path);
            //Debug.Log("noise detected, path exists of length " + length);

            float straightLineDist = Vector3.Distance(transform.position, other.transform.position);
            //Debug.Log("straight line distance: " + straightLineDist);

            if (length < (straightLineDist * 2)) {      // if path less than multiple of straight line distance (PLAY AROUND WITH THE MULTIPLIER)
                //detection.SetDetectingSuspicious(true);
                //detection.suspicousObject = other.transform.parent;
                noiseHeard = true;
                noisePos = other.transform.parent.position;


                /*if (other.transform.parent.tag != "Player") detection.suspicousObject = other.transform.parent;
                else detection.suspicousObject = GameObject.Instantiate(other.transform.parent.GetChild(2), other.transform.parent.position, Quaternion.identity);*/
            }
            else Debug.Log("path too long, can't hear noise");  // does these calculations every frame, could optimise

            // USING HUMANOID NAVMESH SO TINY GAPS WILL OBSTRUCT - KEEP IN MIND
        }
        
        // removed GetDetectingSuspicious check so it will react to new noises while already in investigate state
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
