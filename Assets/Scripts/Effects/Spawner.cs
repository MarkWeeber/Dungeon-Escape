using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [Header("Refer to root sprite renderer to manage X flipping")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Quaternion rotationOnSpawn = Quaternion.identity;
    private Quaternion flippedRotation = Quaternion.Euler(0, 0, 180f);

    public void Spawn()
    {
        if (spriteRenderer != null)
        {
            rotationOnSpawn = spriteRenderer.flipX ? flippedRotation : Quaternion.identity;
        }
        Instantiate(prefab, transform.position, rotationOnSpawn);
    }

}
