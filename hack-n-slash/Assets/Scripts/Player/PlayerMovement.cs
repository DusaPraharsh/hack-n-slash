using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Animator anim;
    public Transform wallCheck;
    public LayerMask wallLayer;
    public BoxCollider2D boxCollider;
    public LayerMask enemyLayer;
    public float range;
    public float colliderDistance;
    public int damage;

    private Health enemyHealth;

    private float horizontal;
    public float speed;

    public float jumpPower;
    private bool isFacingRight = true;
    private float cayoteTime = 0.1f;
    private float cayoteTimeCounter;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    private bool canCombo = false;
    private bool isAttacking = false;

    private bool canDash = true;
    private bool isDashing;
    public float dashPower;
    private float dashTime = 0.4f;
    private float dashCooldown = 1f;
    public bool IsDashing => isDashing;

    private bool isWallSliding;
    private float wallSlidingSpeed = 1f;

    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    private Vector2 wallJumpPower = new Vector2(5f, 10f);

    public AudioClip meeleAttack;
    public AudioClip heavyAttack;
    public AudioClip dashSound;
    public AudioClip jumpSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isDashing)
        {
            return;
        }

        horizontal = Input.GetAxis("Horizontal");

        Jump();

        if(!isWallJumping)
        {
            Flip();
        }

        anim.SetBool("run", horizontal != 0f);
        anim.SetBool("grounded", IsGrounded());

        Attack();
        WallSlide();
        WallJump();

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
            anim.SetTrigger("dash");
        }
    }

    private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }

        if(!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Jump()
    {
        if(IsGrounded())
        {
            cayoteTimeCounter = cayoteTime;
        }
        else
        {
            cayoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if(jumpBufferCounter > 0f && cayoteTimeCounter > 0f)
        {
            SoundManager.instance.PlaySound(jumpSound);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
            jumpBufferCounter = 0f;
        }

        if(Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            cayoteTimeCounter = 0f;
        }
    }

    private void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(!isAttacking)
            {
                SoundManager.instance.PlaySound(meeleAttack);
                anim.SetTrigger("attack_1");
                isAttacking = true;
                StartCoroutine(ComboWindow());
            }
            else if(canCombo)
            {
                SoundManager.instance.PlaySound(meeleAttack);
                anim.SetTrigger("attack_2");
                canCombo = false;
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            SoundManager.instance.PlaySound(heavyAttack);
            anim.SetTrigger("heavy_attack");
        }
    }

    private IEnumerator ComboWindow()
    {
        yield return new WaitForSeconds(0.3f);
        canCombo = true;

        yield return new WaitForSeconds(0.3f);
        canCombo = false;
        isAttacking = false;
    }

    private IEnumerator Dash()
    {
        SoundManager.instance.PlaySound(dashSound);
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
            anim.SetTrigger("wall_slide");
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else{
            wallJumpCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpCounter = 0f;
            SoundManager.instance.PlaySound(jumpSound);
            anim.SetTrigger("wall_jump");

            if(transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool EnemyInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, enemyLayer);
        
        if (hit.collider != null)
        {
            enemyHealth = hit.transform.GetComponent<Health>();
        }
        
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamageEnemy()
    {
        if (EnemyInSight())
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
