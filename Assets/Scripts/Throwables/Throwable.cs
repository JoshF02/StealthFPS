using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Rigidbody _rb;

    public void Init(Transform cam, float initialForce, float initialTorque)
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(cam.forward * initialForce);
        _rb.velocity += cam.parent.GetComponent<CharacterController>().velocity;
        _rb.AddTorque(cam.right * initialTorque);
    }
}
