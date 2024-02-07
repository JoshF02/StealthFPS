using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private float timer = 0f;
    private GameObject thisEnemy;

    void Awake() 
    {
        thisEnemy = this.transform.parent.gameObject;
        //Debug.Log(thisEnemy.name);
    }

    void OnTriggerStay(Collider other) 
    {
        if (other.name == "Player") {
            timer += Time.deltaTime;

            if (timer >= 0.5f) {
                Debug.Log("Player spotted");
                timer = 0f;
            }
        }
        else Debug.Log(other.name);
    }
}
