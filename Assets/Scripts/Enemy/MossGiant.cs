using UnityEngine;

public class MossGiant : Enemy, IDamagable
{
    [Header("Moss Giant Specific parameters")]
    [SerializeField] private float _staggerDuration = 1f;
    public int Health { get; set; }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                animator.SetTrigger("Death");
                SpawnDiamondOnDeath();
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
