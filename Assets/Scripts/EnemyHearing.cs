using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHearing : MonoBehaviour
{
    public enum Alerts
    {
        None,
        EnterHunt,
        EnterCombat,
        Max
    }
    public Alerts AlertHeard { get; private set; } = Alerts.None;
    public bool NoiseHeard { get; private set; } = false;
    public float DisableForSecs { get; private set; } = 0;
    public Vector3 NoisePos { get; private set; } = new();
    private NavMeshAgent _agent;
    private NavMeshPath _path;

    public void StopHearingNoise()
    {
        NoiseHeard = false;
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _path = new NavMeshPath();
    }

    private void Update()
    {
        DisableForSecs -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)  // goes into investigate state to check out noise if it can hear it
    {
        NoiseHeard = false; 

        if ((other.tag == "Sound") && _agent.CalculatePath(other.transform.position, _path) && (_path.status == NavMeshPathStatus.PathComplete))
        {  
            float length = CalculatePathLength(_path);
            float straightLineDist = Vector3.Distance(transform.position, other.transform.position);

            if (length < (straightLineDist * 2))    // if path less than multiple of straight line distance (PLAY AROUND WITH THE MULTIPLIER)
            {      
                NoiseHeard = true;
                NoisePos = other.transform.parent.position;
            } // humanoid navmesh used so tiny gaps will obstruct, and calculations done every frame
        }
        else if (other.tag == "Alert")
        {
            AlertHeard = (other.name == "EnterHuntAlert") ? Alerts.EnterHunt : Alerts.EnterCombat;  // NOTE - DOESNT DO PATHFINDING FOR ALERTS (could do it?)
            //Debug.Log(alertHeard + " alert heard by " + transform.parent.gameObject.name);
        }
        else if (other.tag == "EMP")
        {
            DisableForSecs = 10f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NoiseHeard = false;
        AlertHeard = Alerts.None;
    }

    public float CalculatePathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2) return 0;

        float lengthSoFar = 0.0f;
        Vector3 previousCorner = path.corners[0];

        for (int i = 1; i < path.corners.Length; i++)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
        }

        return lengthSoFar;
    }
}
