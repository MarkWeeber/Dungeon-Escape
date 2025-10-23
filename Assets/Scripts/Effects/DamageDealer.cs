using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageDealer : MonoBehaviour
{
    [SerializeField] protected int damage = 12;
    [SerializeField] protected LayerMask targetMask = 0;
    [SerializeField] protected ushort maxEntitiesDamagedAtOnce = 2;
    protected Collider2D _collider;
    protected IDamagable[] damagables;
    protected IDamagable damagable;

    protected virtual void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        damagables = new IDamagable[maxEntitiesDamagedAtOnce];
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
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

    public virtual void FinishDamageDealing()
    {
        Array.Clear(damagables,0, damagables.Length);
    }
}
