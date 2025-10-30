using UnityEngine;
using UnityEngine.Events;

public class TriggerController : MonoBehaviour
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private bool _runOnce = false;
    [SerializeField] private UnityEvent _onTriggerEnter;
    [SerializeField] private UnityEvent _onTriggerExit;

    private bool _entered, _exited;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_runOnce && _entered) return;
        if (Utils.CheckLayer(_targetMask, collision.gameObject.layer))
        {
            _onTriggerEnter?.Invoke();
            if (_runOnce) _entered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_runOnce && _exited) return;
        if (Utils.CheckLayer(_targetMask, collision.gameObject.layer))
        {
            _onTriggerExit?.Invoke();
            if (_runOnce) _exited = true;
        }
    }
}
