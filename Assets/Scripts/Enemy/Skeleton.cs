using UnityEngine;

public class Skeleton : Enemy, IDamagable
{
    [Header("Skeleton Specifics")]
    [SerializeField] private float _staggerDuration = 0.5f;
    public void TakeDamage(int damage)
    {
        
    }
}
