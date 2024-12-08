using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTarget : Target
{
    [SerializeField] private GameObject bodyPrefab;
    private bool dead = false;

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        //Debug.Log("enemy shot");
        GetComponent<DroneSM>().beenShot = true;
    }

    public override void Die()
    {
        if (!dead) {
            base.Die();
            //Debug.Log("enemy die");

            GameObject body = Instantiate(bodyPrefab, transform.position, quaternion.identity);
            body.name = "Drone Body";
            body.transform.parent = this.transform.parent;
            dead = true;
        }
        else Debug.Log("tried to spawn 2nd body");
    }
}
