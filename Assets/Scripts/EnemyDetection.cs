using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    //private float timer = 0f;
    //private GameObject thisEnemy;
    private bool detectingPlayer = false;

    public bool GetDetectingPlayer() { return detectingPlayer; }

    void Awake() 
    {
        //thisEnemy = this.transform.parent.gameObject;
        //Debug.Log(thisEnemy.name);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") {
            detectingPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player") {
            detectingPlayer = false;
        }
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
