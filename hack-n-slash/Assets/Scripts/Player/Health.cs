using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    private Animator anim;
    private bool dead;

    public Behaviour[] components;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("death");

                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }

                dead = true;
            }
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        dead = false;
        currentHealth = maxHealth;
        anim.ResetTrigger("death");
        anim.Play("idle");

        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }
    }
}
