using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTarget : Target
{
    [SerializeField] private GameObject bodyPrefab;

    /*public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Debug.Log("enemy take damage");
    }*/

    public override void Die()
    {
        base.Die();
        //Debug.Log("enemy die");

        GameObject body = Instantiate(bodyPrefab, transform.position, quaternion.identity);
        body.name = "Drone Body";
    }
}
