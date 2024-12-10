using UnityEngine;

public class Target : MonoBehaviour
{
    private float _health;

    public virtual void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0f) Die();
    }

    public virtual void Die()
    {
        GameManager.Instance.IncreaseScoreBy(25);
        //Debug.Log("object destroyed");
        Destroy(gameObject);
    }
}
