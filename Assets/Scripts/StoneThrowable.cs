using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneThrowable : MonoBehaviour
{
    private Transform cam;
    private Rigidbody rb;
    private bool hasCollided = false;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(cam.forward * 2500.0f);
        rb.velocity += cam.parent.GetComponent<CharacterController>().velocity;
        rb.AddTorque(cam.right * 20f);
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
