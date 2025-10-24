using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class DamageObject : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 1;           // ������� ����� ������� ������
    [SerializeField] private bool destroyOnTouch = false; // ���������� �� ������ ����� �������
    [SerializeField] private bool continuousDamage = false; // �������� ���� ���������, ���� ����� ��������
    [SerializeField] private float damageInterval = 1f;   // �������� ����� ������� ��� ����������� �������

    private bool canDealDamage = true; // ������ �� ����������� ���������� �����

    private void Reset()
    {
        // ��������, ��� ��������� �������� ���������
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();

        if (player != null && canDealDamage)
        {
            DealDamage(player);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (continuousDamage)
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();

            if (player != null && canDealDamage)
            {
                DealDamage(player);
            }
        }
    }

    private void DealDamage(PlayerHealth player)
    {
        // �������� ���� ������, ������� ������� ��������� �����
        player.TakeDamage(damage);

        

        if (destroyOnTouch)
            Destroy(gameObject);

        if (continuousDamage)
            StartCoroutine(DamageCooldown());
        else
            canDealDamage = false;
    }

    private System.Collections.IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(damageInterval);
        canDealDamage = true;
    }
}
