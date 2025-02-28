using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public static event Action OnPlayedDied;

    public HealthUI healthUI;
    public GameObject gameOverScreen;
    private Animator animator; // Добавляем аниматор

    private void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
        animator = GetComponent<Animator>(); // Получаем компонент Animator
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damageToPlayer);
        }
        else if (collision.CompareTag("Death"))
        {
            Die();
        }
        else if (collision.CompareTag("Finish"))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (currentSceneName == "Level1")
            {
                SceneManager.LoadScene(2);
            }
            else if (currentSceneName == "Level2")
            {
                SceneManager.LoadScene(3);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);
        SoundEffectManager.Instance.PlayDamageSound();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SoundEffectManager.Instance.PlayGameOverSound();
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        OnPlayedDied?.Invoke();

        StartCoroutine(DisablePlayerAfterAnimation()); // Ждем анимацию перед отключением
    }

    private IEnumerator DisablePlayerAfterAnimation()
    {
        yield return new WaitForSeconds(1f); // Заменить на длину анимации смерти
        gameObject.SetActive(false);
    }

}
