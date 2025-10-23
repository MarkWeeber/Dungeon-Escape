using UnityEngine;

public class SpiderStationary : IDamagable
{
    private int health;
    public int Health { get => health; set => health = value; }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                
            }
            else
            {
                
            }
        }
    }
}
