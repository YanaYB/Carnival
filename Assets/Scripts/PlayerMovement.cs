using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator animator;
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f; // ��������� ���������
    private float currentMoveSpeed; // ������� �������� ��������
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


    private bool isSprinting = false; 

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // �������� � ������ � FixedUpdate
        rb.velocity = new Vector2(horizontalMovement * currentMoveSpeed, rb.velocity.y);

        // ������� ������� � ����������� �� ����������� ��������
        if (horizontalMovement > 0) // �������� ������
        {
            transform.localScale = new Vector3(1, 1, 1); // ������� �������
        }
        else if (horizontalMovement < 0) // �������� �����
        {
            transform.localScale = new Vector3(-1, 1, 1); // �������� ������ �� ��� X
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

        // ��������� ��������� ���������
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", Mathf.Abs(horizontalMovement));
        animator.SetBool("isGrounded", isGrounded);

        // ���������� ������� jump, ���� �������� �����������
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
                // ���������, ����� �� �� �������� 
                if (isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    jumpsRemaining--;
                    animator.SetTrigger("jump");
                }
            }
            else if (context.canceled && rb.velocity.y > 0)
            {
                // ��������� ������ ������, ���� ������ ������ �������� ������ �������
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
            currentMoveSpeed = moveSpeed * sprintMultiplier; // ����������� ��������
            animator.SetBool("isSprinting", true); // �������� �������� �������
        }
        else if (context.canceled)
        {
            isSprinting = false;
            currentMoveSpeed = moveSpeed; // ���������� ������� ��������
            animator.SetBool("isSprinting", false); // ��������� �������� �������
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
                jumpsRemaining = maxJumps; // ���������� ������ ��� �����������
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}