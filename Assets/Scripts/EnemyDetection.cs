using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private bool detectingPlayer = false;
    private bool detectingSuspicious = false;
    [HideInInspector] public Transform suspicousObject = null;

    public bool GetDetectingPlayer() { return detectingPlayer; }
    public bool GetDetectingSuspicious() { return detectingSuspicious; }
    public void SetDetectingSuspicious(bool value) { detectingSuspicious = value; } 

    public void SetMoreAware()  // initial radius should be more aware size
    { 
        Debug.Log("more aware");
        GetComponent<SphereCollider>().radius *= 2;
    }
    public void SetLessAware()
    { 
        Debug.Log("less aware");
        GetComponent<SphereCollider>().radius *= 0.5f;
    } 

    void Awake() 
    {
    }

    void OnTriggerEnter(Collider other) // NEED TO ADD RAYCASTING FOR PROPER VISUAL DETECTION, + add audio detection + alerts from other drones
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
    }
}
