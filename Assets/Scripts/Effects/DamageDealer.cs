using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damage = 12;
    [SerializeField] private LayerMask targetMask = 0;
    [SerializeField] private ushort maxEntitiesDamagedAtOnce = 2;
    private Collider2D _collider;
    private IDamagable[] damagables;
    private IDamagable damagable;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        damagables = new IDamagable[maxEntitiesDamagedAtOnce];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
        {
            if (collision.gameObject.TryGetComponent(out damagable))
            {
                if (damagables.TryAddNewItem(damagable))
                {
                    damagable.TakeDamage(damage);
                }
            }
        }
    }

    public void FinishDamageDealing()
    {
        Array.Clear(damagables,0, damagables.Length);
    }
}
