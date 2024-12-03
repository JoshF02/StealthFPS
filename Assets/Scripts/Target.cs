using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;

    public virtual void TakeDamage (float amount)
    {
        health -= amount;
        if (health <= 0f) Die();
    }

    public virtual void Die ()
    {
        GameManager.Instance.IncreaseScoreBy(25);
        //Debug.Log("object destroyed");
        Destroy(gameObject);
    }
}
