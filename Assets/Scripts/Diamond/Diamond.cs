using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private int _diamondWorth = 1;
    public int DiamondWorth { get => _diamondWorth; set => _diamondWorth |= value; }
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CheckLayer(_targetMask, collision.gameObject.layer))
        {
            if (collision.TryGetComponent<Player>(out player))
            {
                player.Diamonds += _diamondWorth;
                Destroy(gameObject);
            }
        }
    }

}
