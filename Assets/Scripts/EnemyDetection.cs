using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    //private float timer = 0f;
    //private GameObject thisEnemy;
    private bool detectingPlayer = false;
    private bool detectingSuspicious = false;
    [HideInInspector] public Transform suspicousObject = null;

    public bool GetDetectingPlayer() { return detectingPlayer; }
    public bool GetDetectingSuspicious() { return detectingSuspicious; }
    public void SetDetectingSuspicious(bool value) { detectingSuspicious = value; } 

    void Awake() 
    {
        //thisEnemy = this.transform.parent.gameObject;
        //Debug.Log(thisEnemy.name);
    }

    void OnTriggerEnter(Collider other) // NEED TO ADD RAYCASTING FOR PROPER DETECTION + change awareness size depending on state
    {
        if (other.name == "Player") {
            detectingPlayer = true;
        }
        else if (other.tag == "Suspicious" && !detectingSuspicious) {
            detectingSuspicious = true;
            suspicousObject = other.transform;
        }
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
        /*else if (other.tag == "Suspicious") {   // could probably remove
            detectingSuspicious = false;
            suspicousObject = null;
        }*/
    }

    /*void Update()
    {
        if(detectingPlayer) Debug.Log("detecting");
    }*/

    /*void OnTriggerStay(Collider other) 
    {
        if (other.name == "Player") {
            timer += Time.deltaTime;

            if (timer >= 0.5f) {
                Debug.Log("Player spotted");
                timer = 0f;
            }
        }
        else Debug.Log(other.name);
    }*/
}
