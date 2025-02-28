using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // Скорость полета
    public float lifetime = 2f; // Время жизни огонька

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0); // Огонёк летит по горизонтали

        Destroy(gameObject, lifetime); // Уничтожаем огонёк через время
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Уничтожаем врага
            Destroy(gameObject); // Огонёк тоже исчезает
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // Если попал в стену или землю, исчезает
        }
    }
}
