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

    [Header("Resources")]
    public int totalResourcesRequired = 5; // Сколько всего нужно собрать ресурсов
    private int collectedResources = 0; // Сколько уже собрано
    public GameObject levelExit; // Объект выхода на след. уровень

    [Header("Resource UI")]
    public TMP_Text resourceInfoText; // Просто один TextMeshPro текст на сцене
    public float messageShowTime = 10f; // Время показа сообщения


    void Start()
    {
        currentMoveSpeed = moveSpeed;
        rb = GetComponent<Rigidbody2D>();

        GameObject startObject = GameObject.FindGameObjectWithTag("Start");
        if (startObject != null)
            transform.position = startObject.transform.position - new Vector3(-2, 1, 0);
        if (resourceInfoText != null)
            resourceInfoText.text = "";
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalMovement * currentMoveSpeed, rb.velocity.y);

        if (horizontalMovement > 0)
        {
            transform.localScale = new Vector3(3, 3, 5);
        }
        else if (horizontalMovement < 0)
        {
            transform.localScale = new Vector3(-3, 3, 5);
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

    // Метод для сбора ресурсов
    public void CollectResource(GameObject resourceObject)
    {
        collectedResources++;

        // Получаем имя ресурса из тега или компонента
        string resourceName = resourceObject.name.Replace("(Clone)", "");

        // Обновляем текст
        if (resourceInfoText != null)
        {
            resourceInfoText.text = $"{resourceName}!";

            // Прячем текст через заданное время
            CancelInvoke(nameof(HideResourceText));
            Invoke(nameof(HideResourceText), messageShowTime);
        }

        Destroy(resourceObject);

        if (collectedResources >= totalResourcesRequired && levelExit != null)
        {
            levelExit.SetActive(true);
        }
    }

    private void HideResourceText()
    {
        if (resourceInfoText != null)
            resourceInfoText.text = "";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            if (collectedResources >= totalResourcesRequired)
            {
                string current = SceneManager.GetActiveScene().name;

                switch (current)
                {
                    case "Level2":
                        SceneManager.LoadScene("Level1");
                        break;
                    case "Level1":
                        SceneManager.LoadScene("Exit");
                        break;
                    default:
                        SceneManager.LoadScene("Level2");
                        break;
                }
            }
            else
            {
                
                Invoke(nameof(HideResourceText), messageShowTime);
            }
        }
        else if (other.CompareTag("Resource"))
        {
            CollectResource(other.gameObject);
        }
    }

    // Остальные методы остаются без изменений
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