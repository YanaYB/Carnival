using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animator;
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f; // Множитель ускорения
    private float currentMoveSpeed; // Текущая скорость движения
    float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 2;
    private int jumpsRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallGravityMult = 2f;


    [Header("Death")]
    public float deathDelay = 10f; // Задержка перед переходом на сцену Game Over
    private bool isDead = false; // Флаг, указывающий, что игрок мертв
    public int gameOverSceneIndex = 4; // Индекс сцены Game Over
    private float deathTimer;

    private bool isSprinting = false; 

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        animator = GetComponent<Animator>();

        GameObject startObject = GameObject.FindGameObjectWithTag("Start");
        transform.position = startObject.transform.position - new Vector3(-2, 1, 0);

    }

    void FixedUpdate()
    {
        // Движение и физика в FixedUpdate
        rb.velocity = new Vector2(horizontalMovement * currentMoveSpeed, rb.velocity.y);

        // Поворот спрайта в зависимости от направления движения
        if (horizontalMovement > 0) // Движение вправо
        {
            transform.localScale = new Vector3(1, 1, 1); // Обычный масштаб
        }
        else if (horizontalMovement < 0) // Движение влево
        {
            transform.localScale = new Vector3(-1, 1, 1); // Отразить спрайт по оси X
        }

        // Falling gravity
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallGravityMult; // Fall faster and faster
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed)); // Max fall speed
        }
        else
        {
            rb.gravityScale = baseGravity;
        }

        GroundCheck();

        // Обновляем параметры аниматора
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", Mathf.Abs(horizontalMovement));
        animator.SetBool("isGrounded", isGrounded);

        // Сбрасываем триггер jump, если персонаж приземлился
        if (isGrounded)
        {
            animator.ResetTrigger("jump");
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                // Проверяем, можем ли мы прыгнуть 
                if (isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    jumpsRemaining--;
                    animator.SetTrigger("jump");
                }
            }
            else if (context.canceled && rb.velocity.y > 0)
            {
                // Уменьшаем высоту прыжка, если кнопка прыжка отпущена раньше времени
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
            currentMoveSpeed = moveSpeed * sprintMultiplier; // Увеличиваем скорость
            animator.SetBool("isSprinting", true); // Включаем анимацию спринта
        }
        else if (context.canceled)
        {
            isSprinting = false;
            currentMoveSpeed = moveSpeed; // Возвращаем обычную скорость
            animator.SetBool("isSprinting", false); // Выключаем анимацию спринта
        }
    }

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);

        if (isGrounded)
        {
            if (!wasGrounded)
            {
                jumpsRemaining = maxJumps; // Сбрасываем прыжки при приземлении
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
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
        }else if (collision.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // Если игрок уже мертв, выходим

        isDead = true; // Устанавливаем флаг смерти
        animator.SetBool("isDead", true); // Запускаем анимацию смерти
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.isKinematic = true;
        //deathTimer = deathDelay;
    }
}