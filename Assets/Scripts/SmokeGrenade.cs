using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    [SerializeField] private GameObject smoke;
    private GameObject smokeObj;
    private Transform player;
    private Rigidbody rb;
    private float timer = 0f;
    private bool hasSpawned = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(player.forward * 2500.0f);
        rb.velocity += player.GetComponent<CharacterController>().velocity;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3.0f && !hasSpawned) {
            smokeObj = Instantiate(smoke, transform.position, Quaternion.identity);  // will need to change smoke texture
            GetComponent<MeshRenderer>().enabled = false;
            hasSpawned = true;
        }

        if (timer > 18.0f) {
            Destroy(smokeObj);
            Destroy(gameObject);
        }
    }
}
