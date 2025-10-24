using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public bool invincible = false;
    private bool canMove = true;
    public static event Action OnPlayedDied;

    public HealthUI healthUI;
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Attack playerAttack;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerAttack = GetComponent<Attack>();
        healthUI.SetMaxHearts(maxHealth);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);
        
       

        if (currentHealth <= 0)
        {
            StartCoroutine(WaitToDead());
        }
        else
        {
            StartCoroutine(Stun(0.25f));
            StartCoroutine(MakeInvincible(1f));
        }
    }


    IEnumerator WaitToDead()
    {
        animator.SetBool("IsDead", true);
        canMove = false;
        invincible = true;

        if (playerController != null)
            playerController.enabled = false;
        if (playerAttack != null)
            playerAttack.enabled = false;

        yield return new WaitForSeconds(0.4f);

        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(2);
    }

    IEnumerator Stun(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    IEnumerator MakeInvincible(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }

}
