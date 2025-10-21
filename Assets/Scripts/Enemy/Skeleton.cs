using UnityEngine;

public class Skeleton : Enemy, IDamagable
{
    [Header("Skeleton Specific parameters")]
    [SerializeField] private float _staggerDuration = 0.5f;
    public void TakeDamage(int damage)
    {
        waitingTimer = _staggerDuration;
        animator.SetTrigger("Hit");
    }
}
