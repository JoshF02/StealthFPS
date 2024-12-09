using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private readonly float viewDistance = 10.0f;
    private readonly float viewAngle = 32;
    private bool detectingPlayer = false;
    [HideInInspector] public bool DetectingSuspicious { get; private set; } = false;
    [HideInInspector] public bool DetectingDecoy { get; private set; } = false;
    [HideInInspector] public Transform SuspicousObject { get; private set; } = null;
    [HideInInspector] public Transform Decoy { get; private set; } = null;
    [SerializeField] private LayerMask detectionLayerMask;

    public bool GetDetectingPlayer(Vector3 enemyPos, Vector3 playerPos)
    {
        if (detectingPlayer) return true;

        return (Vector3.Distance(enemyPos, playerPos) < 3.0f);  // detects player if very close
    }
    public bool GetDetectingTarget(Vector3 enemyPos, Vector3 targetPos, bool targetIsPlayer)
    {
        if (targetIsPlayer) return GetDetectingPlayer(enemyPos, targetPos);
        else return DetectingDecoy;
    }
    public void StartDetectingSuspicious(Transform target)
    {
        SuspicousObject = target;
        DetectingSuspicious = true;
    }
    public void StopDetectingSuspicious(bool shouldDisableBool = false)
    {
        SuspicousObject = null;
        if (shouldDisableBool) DetectingSuspicious = false;
    }
    public void StopDetectingDecoy(bool shouldDisableBool = false)
    {
        Decoy = null;
        if (shouldDisableBool) DetectingDecoy = false;
    }

    public void SetMoreAware()  // initial radius should be more aware size
    { 
        Debug.Log("more aware");
        //GetComponent<SphereCollider>().radius *= 2;
        //GetComponent<BoxCollider>().size *= 2;
    }
    public void SetLessAware()
    { 
        Debug.Log("less aware");
        //GetComponent<SphereCollider>().radius *= 0.5f;
        //GetComponent<BoxCollider>().size *= 0.5f;
    } 

    void OnTriggerEnter(Collider other)
    {                                   
        if (other.name == "Player" && !detectingPlayer) {
            detectingPlayer = CanSeeObject(other.transform);
        }
        else if (other.tag == "Suspicious" && !DetectingSuspicious) {
            DetectingSuspicious = CanSeeObject(other.transform);
            if (DetectingSuspicious) SuspicousObject = other.transform;
        }
        else if (other.tag == "Decoy") {
            //Debug.Log("detecting decoy");
            DetectingDecoy = CanSeeObject(other.transform);
            if (DetectingDecoy) Decoy = other.transform;
        }
        // only objects with rigidbodies set off triggers
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player" && !detectingPlayer) {
            detectingPlayer = CanSeeObject(other.transform);
        }
        else if (other.tag == "Suspicious" && !DetectingSuspicious) {
            DetectingSuspicious = CanSeeObject(other.transform);
            if (DetectingSuspicious) SuspicousObject = other.transform;
        }
        else if (other.tag == "Decoy") {
            //Debug.Log("detecting decoy");
            DetectingDecoy = CanSeeObject(other.transform);
            if (DetectingDecoy) Decoy = other.transform;
        }
    }

    void OnTriggerExit(Collider other)  // SHOULD USE BETTER WAY OF BREAKING DETECTION
    {
        if (other.name == "Player") {
            detectingPlayer = false;
        }
        /*else if (other.tag == "Decoy") {
            Debug.Log("stopped detecting decoy");
            detectingDecoy = false;
        }*/
    }

    bool CanSeeObject(Transform obj)
    {
        if (Vector3.Distance(transform.position, obj.position) > viewDistance) return false;    // out of range

        Vector3 dir = (obj.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);

        if (angle > viewAngle) return false; // out of view cone

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewDistance, detectionLayerMask)) {   // obstructed
            if (hit.collider.gameObject.name != obj.name) {
                return false;
            }
        }

        return true;    // could change to detection meter filling up, rather than instant
    }

    void OnDrawGizmos() // shows viewcone lines
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.parent.forward * viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, viewAngle, 0) * transform.parent.forward * viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -viewAngle, 0) * transform.parent.forward * viewDistance);
    }
}
