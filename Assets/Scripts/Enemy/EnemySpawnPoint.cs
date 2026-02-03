using System.Collections;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private string enemyType;
    public string EnemyType => enemyType;
    [SerializeField] private float spawnInterval = 1f;

    private bool isSpawning = false;

    void Start()
    {
        StartSpawn();
    }

    public void StartSpawn()
    {
        isSpawning = true;
        StartCoroutine(SpawnLoop());
    }
    public void StopSpawn()
    {
        isSpawning = false;
    }

    IEnumerator SpawnLoop()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
            if (spawner != null)
            {
                spawner.SpawnEnemy(enemyType, transform.position);
            }
            else
            {
                Debug.LogWarning("EnemySpawner not found in the scene.");
            }
        }
    }
}

