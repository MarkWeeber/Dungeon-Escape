using UnityEngine;

public class Skeleton : Enemy, IDamagable
{
    [Header("Skeleton Specific parameters")]
    [SerializeField] private float _staggerDuration = 0.5f;
    public int Health { get; set; }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                animator.SetTrigger("Death");
                SpawnDiamonOnDeath();
                alive = false;
            }
            else
            {
                waitingTimer = _staggerDuration;
                animator.SetTrigger("Hit");
                CheckBehindIfNotYetAlerted();
            }
        }
    }
}
