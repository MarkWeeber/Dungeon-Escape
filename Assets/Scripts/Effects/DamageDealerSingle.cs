using UnityEngine;

public class DamageDealerSingle : DamageDealer
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private Vector2 travelVector = Vector2.right;

    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, lifeTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
        {
            if (collision.gameObject.TryGetComponent(out damagable))
            {
                damagable.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (travelVector != Vector2.zero)
        {
            transform.Translate(travelVector * Time.deltaTime);
        }
    }
}
