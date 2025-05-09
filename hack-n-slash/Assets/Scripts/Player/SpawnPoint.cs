using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Transform spawnPoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindFirstObjectByType<UIManager>();
    }

    public void Spawn()
    {
        if (spawnPoint == null)
        {
            uiManager.GameOver();
            
            return;
        }

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
