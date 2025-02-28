using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int enemyCount = 5;
    private GameObject[] enemies;

    void Start()
    {
        SpawnEnemies();
        PlayerHealth.OnPlayedDied += OnPlayerDied;
    }

    void OnDestroy()
    {
        PlayerHealth.OnPlayedDied -= OnPlayerDied;
    }

    void SpawnEnemies()
    {
        enemies = new GameObject[enemyCount];
        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            enemies[i] = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    void OnPlayerDied()
    {
        RemoveAllEnemies();
        ResetHealthUI();
    }

    void RemoveAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }

    void ResetHealthUI()
    {
        HealthUI healthUI = FindObjectOfType<HealthUI>();
        if (healthUI != null)
        {
            healthUI.UpdateHearts(0); 
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
