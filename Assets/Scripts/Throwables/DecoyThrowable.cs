using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyThrowable : Throwable
{
    [SerializeField] private GameObject decoyPrefab;

    void OnCollisionEnter(Collision collision)  // only spawns decoy once it hits ground
    {
        if (collision.transform.tag != "Ground") return;

        Debug.Log("spawning decoy now");
        GameObject decoyObj = Instantiate(decoyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
