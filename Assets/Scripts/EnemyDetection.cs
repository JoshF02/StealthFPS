using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private bool detectingPlayer = false;
    private bool detectingSuspicious = false;
    [HideInInspector] public Transform suspicousObject = null;
    private readonly float viewDistance = 10.0f;
    private readonly float viewAngle = 40;
    [SerializeField] public LayerMask detectionLayerMask;

    public bool GetDetectingPlayer() { return detectingPlayer; }
    public bool GetDetectingSuspicious() { return detectingSuspicious; }
    public void SetDetectingSuspicious(bool value) { detectingSuspicious = value; } 

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

    void OnTriggerEnter(Collider other) // NEED TO add audio detection + alerts from other drones
    {                                   
        if (other.name == "Player" && !detectingPlayer) {
            detectingPlayer = CanSeeObject(other.transform);
        }
        else if (other.tag == "Suspicious" && !detectingSuspicious) {
            detectingSuspicious = CanSeeObject(other.transform);
            if (detectingSuspicious) suspicousObject = other.transform;
        }
        // only objects with rigidbodies set off triggers
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player" && !detectingPlayer) {
            detectingPlayer = CanSeeObject(other.transform);
        }
        else if (other.tag == "Suspicious" && !detectingSuspicious) {
            detectingSuspicious = CanSeeObject(other.transform);
            if (detectingSuspicious) suspicousObject = other.transform;
        }
    }

    void OnTriggerExit(Collider other)  // SHOULD USE BETTER WAY OF BREAKING DETECTION
    {
        if (other.name == "Player") {
            detectingPlayer = false;
        }
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
