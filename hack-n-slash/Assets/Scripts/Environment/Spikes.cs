using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();

            if (playerMovement != null && !playerMovement.IsDashing)
            {
                collision.GetComponent<Health>()?.TakeDamage(damage);
            }
            else
            {
            }
        }
    }
}
