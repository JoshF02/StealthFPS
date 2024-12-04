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
    [SerializeField] private float viewDistance = 10.0f;

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

    /*void OnTriggerEnter(Collider other) // NEED TO ADD RAYCASTING FOR PROPER VISUAL DETECTION, + add audio detection + alerts from other drones
    {                                   
        if (other.name == "Player") {
            detectingPlayer = true;
        }
        else if (other.tag == "Suspicious" && !detectingSuspicious) {
            detectingSuspicious = true;
            suspicousObject = other.transform;
        }
        // only objects with rigidbodies set off triggers
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Suspicious" && !detectingSuspicious) {
            detectingSuspicious = true;
            suspicousObject = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player") {
            detectingPlayer = false;
        }
    }*/

    void OnDrawGizmos() // shows viewcone lines
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.parent.forward * viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 40, 0) * transform.parent.forward * viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -40, 0) * transform.parent.forward * viewDistance);
    }
}
