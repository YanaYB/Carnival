using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image heartPrefab;

    private List<Image> hearts = new List<Image>();

    public void SetMaxHearts(int maxHearts)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();

        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(heartPrefab, transform);
            hearts.Add(newHeart);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].enabled = i < currentHealth;
        }
    }
}
