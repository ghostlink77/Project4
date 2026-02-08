/*
 * 적 스폰 지점 관리 스크립트
 */
using System.Collections;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private string enemyType;
    public string EnemyType { get => enemyType; set => enemyType = value; }
    [SerializeField] private float spawnInterval = 1f;

    private bool isSpawning = false;

    private EnemySpawner spawner;
    private CircleCollider2D spawnRangeCollider;

    private void Awake()
    {
        spawnRangeCollider = GetComponentInChildren<CircleCollider2D>();
    }

    private void Start()
    {
        spawner = EnemySpawner.Instance;
    }

    public void StartSpawn()
    {
        isSpawning = true;
        StartCoroutine(SpawnLoop());
    }
    public void StopSpawn()
    {
        isSpawning = false;
        StopCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(GetRandomInterval());
            if (spawner != null)
            {
                spawner.SpawnEnemy(enemyType, GetRandomPosition());
            }
            else
            {
                Debug.LogWarning("EnemySpawner not found in the scene.");
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        float radius = spawnRangeCollider.radius;
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        return transform.position + new Vector3(randomPoint.x, randomPoint.y, 0f);
    }

    private float GetRandomInterval()
    {
        return Random.Range(spawnInterval * 0.5f, spawnInterval * 2f);
    }   
}

