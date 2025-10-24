using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class DamageObject : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 1;           // Сколько урона наносит объект
    [SerializeField] private bool destroyOnTouch = false; // Уничтожить ли объект после касания
    [SerializeField] private bool continuousDamage = false; // Наносить урон постоянно, пока игрок касается
    [SerializeField] private float damageInterval = 1f;   // Интервал между уронами при непрерывном касании

    private bool canDealDamage = true; // Защита от мгновенного повторного урона

    private void Reset()
    {
        // Убедимся, что коллайдер настроен правильно
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
        // Вызываем урон игроку, передаём позицию источника урона
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
