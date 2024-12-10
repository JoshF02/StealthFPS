using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTarget : Target
{
    [SerializeField] private GameObject _bodyPrefab;
    private bool _dead = false;

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        GetComponent<DroneSM>().BeenShot = true;
    }

    public override void Die()
    {
        if (_dead) return;

        base.Die();
        _dead = true;

        if (GameManager.Instance.NoBodies)
        {
            Debug.Log("no bodies perk enabled");
            return;
        }

        GameObject body = Instantiate(_bodyPrefab, transform.position, quaternion.identity);
        body.name = "Drone Body";
        body.transform.parent = this.transform.parent;
    }
}
