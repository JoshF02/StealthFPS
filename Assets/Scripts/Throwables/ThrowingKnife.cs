using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : Throwable
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.childCount > 0 && collision.transform.root.GetChild(0).TryGetComponent<EnemyTarget>(out EnemyTarget target))
        {
            //Debug.Log("throwing knife hit enemy");
            target.TakeDamage(999999);
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log("throwing knife hit something else");
            Destroy(gameObject);
        }
    }
}
