using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneThrowable : MonoBehaviour
{
    private Transform player;
    private Rigidbody rb;
    private bool hasCollided = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(player.forward * 2500.0f);
        rb.velocity += player.GetComponent<CharacterController>().velocity;
    }

    void OnCollisionEnter(Collision collision)  // only generates sound once it hits ground/object
    {
        if (!hasCollided) {
            Debug.Log("generating noise now");
            SoundGenerator.Instance.GenerateSound(transform, 20, 0.5f);
            hasCollided = true;
        }
    }
}
