using UnityEngine;

public class Enemy_sideways : MonoBehaviour
{
    public int damage;
    public float movementDistance;
    public float speed;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                movingLeft = true;
            }
        }
    }

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
