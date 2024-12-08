using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Rigidbody rb;

    public void Init(Transform cam, float initialForce, float initialTorque)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(cam.forward * initialForce);
        rb.velocity += cam.parent.GetComponent<CharacterController>().velocity;
        rb.AddTorque(cam.right * initialTorque);
    }
}
