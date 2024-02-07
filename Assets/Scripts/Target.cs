using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;

    public void TakeDamage (float amount)
    {
        health -= amount;
        if (health <= 0f) Die();
    }

    public void Die ()
    {
        GameManager.Instance.IncreaseScoreBy(25);
        //Debug.Log("object destroyed");
        Destroy(gameObject);
    }
}
