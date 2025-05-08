using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounce;
    public Animator anim;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }
}
