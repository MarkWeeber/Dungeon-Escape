using System;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class EnemyVision : MonoBehaviour
{
    [SerializeField] private LayerMask targetMask = int.MaxValue;
    private Collider2D _collider;

    public Action<Transform> OnVisionEnter;
    public Action OnVisionExit;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
        {
            OnVisionEnter?.Invoke(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Utils.CheckLayer(targetMask, collision.gameObject.layer))
        {
            OnVisionExit?.Invoke();
        }
    }
}

