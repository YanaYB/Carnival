using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

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


    void Start()
    {
        currentMoveSpeed = moveSpeed;
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalMovement * currentMoveSpeed, rb.velocity.y);

        if (horizontalMovement > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontalMovement < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
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
                if (isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    jumpsRemaining--;
                    if (SoundEffectManager.Instance != null)
                        SoundEffectManager.Instance.PlayJumpSound();
                }
            }
            else if (context.canceled && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsRemaining--;
            }
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           
            currentMoveSpeed = moveSpeed * sprintMultiplier;
        }
        else if (context.canceled)
        {
            
            currentMoveSpeed = moveSpeed;
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