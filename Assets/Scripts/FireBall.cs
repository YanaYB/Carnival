using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // �������� ������
    public float lifetime = 2f; // ����� ����� �������

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0); // ����� ����� �� �����������

        Destroy(gameObject, lifetime); // ���������� ����� ����� �����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // ���������� �����
            Destroy(gameObject); // ����� ���� ��������
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // ���� ����� � ����� ��� �����, ��������
        }
    }
}
