using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Transform spawnPoint;
    private Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
    }

    public void Spawn()
    {
        transform.position = spawnPoint.position;
        playerHealth.Respawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "SpawnPoint")
        {
            spawnPoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
        }
    }
}
