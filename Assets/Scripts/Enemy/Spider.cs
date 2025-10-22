public class Spider : Enemy, IDamagable
{
    public int Health { get; set; }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                animator.SetTrigger("Death");
                alive = false;
            }
            else
            {
                CheckBehindIfNotYetAlerted();
            }
        }
    }
}
