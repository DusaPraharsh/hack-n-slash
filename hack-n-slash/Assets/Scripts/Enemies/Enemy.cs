using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float attackCooldown;
    public float range;
    public float colliderDistance;
    public int damage;
    public BoxCollider2D boxCollider;
    public LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;

    private EnemyPatrol enemyPatrol;

    public Canvas canvas;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        canvas.enabled = false;
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            canvas.enabled = true;
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * -transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        
        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * -transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
