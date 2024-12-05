using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHearing : MonoBehaviour
{
    EnemyDetection detection;
    NavMeshAgent agent;
    NavMeshPath path;
    //public Transform target;

    void Start()
    {
        detection = transform.GetChild(2).GetComponent<EnemyDetection>();
        /*agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();

        agent.CalculatePath(target.position, path);

        Debug.Log("STATUS: " + path.status);
        Debug.Log("LENGTH: " + CalculatePathLength(path));*/
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Sound") {
            //Debug.Log("sound detected by " + gameObject);

            if (!detection.GetDetectingSuspicious()) {  // goes into investigate state to check out noise
                Debug.Log("INVESTIGATING NOISE");
                detection.SetDetectingSuspicious(true);
                detection.suspicousObject = other.transform.parent; // wont work with player as parent, need to set to null after 1 pathfind
            }
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

    // enemy ontriggerstay function - calculate navmesh path + check length using code above, if less than (radius * scaledownfactor) then detect it
}
