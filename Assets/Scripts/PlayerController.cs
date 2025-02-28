using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    private float currentMoveSpeed;
    private float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 1;
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

    private bool isSprinting = false;

    [Header("Attack")]
    public GameObject fireballPrefab; // Префаб огонька
    public float fireballSpeed = 10f; // Скорость огонька
    public Vector2 fireballOffset = new Vector2(0.5f, 0f); // Смещение относительно игрока

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        animator = GetComponent<Animator>();

        GameObject startObject = GameObject.FindGameObjectWithTag("Start");
        transform.position = startObject.transform.position - new Vector3(-2, 1, 0);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalMovement * currentMoveSpeed, rb.velocity.y);

        if (horizontalMovement > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalMovement < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallGravityMult;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }

        GroundCheck();

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", Mathf.Abs(horizontalMovement));
        animator.SetBool("isGrounded", isGrounded);

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
                    SoundEffectManager.Instance.PlayJumpSound();
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
            currentMoveSpeed = moveSpeed * sprintMultiplier;
            animator.SetBool("isSprinting", true);
        }
        else if (context.canceled)
        {
            isSprinting = false;
            currentMoveSpeed = moveSpeed;
            animator.SetBool("isSprinting", false);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetBool("isAtack", true);
            StartCoroutine(ShootFireball()); // Выпускаем огонь с задержкой
        }
        else if (context.canceled)
        {
            animator.SetBool("isAtack", false);
        }
    }

    private IEnumerator ShootFireball()
    {
        yield return new WaitForSeconds(0.2f); // Подождем, чтобы синхронизировать с анимацией

        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 firePosition = transform.position + new Vector3(fireballOffset.x * direction, fireballOffset.y, 0);

        GameObject fireball = Instantiate(fireballPrefab, firePosition, Quaternion.identity);
        Fireball fireballScript = fireball.GetComponent<Fireball>();

        fireballScript.speed = fireballSpeed * direction;

        if (direction < 0)
        {
            fireball.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            jumpsRemaining = maxJumps;
        }
    }
}
