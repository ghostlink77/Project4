using System.Collections;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType = EnemyType.Drone1;
    public EnemyType EnemyType { get => _enemyType; set => _enemyType = value; }
    [SerializeField] private float spawnInterval = 1f;

    private bool _isSpawning = false;

    private EnemySpawner _spawner;
    private CircleCollider2D _spawnRangeCollider;

    private void Awake()
    {
        _spawnRangeCollider = GetComponentInChildren<CircleCollider2D>();
    }

    private void Start()
    {
        _spawner = EnemySpawner.Instance;
    }

    public void StartSpawn()
    {
        _isSpawning = true;
        StartCoroutine(SpawnLoop());
    }
    public void StopSpawn()
    {
        _isSpawning = false;
        StopCoroutine("SpawnLoop");
    }

    IEnumerator SpawnLoop()
    {
        while (_isSpawning)
        {
            yield return new WaitForSeconds(GetRandomInterval());
            if (_spawner != null)
            {
                _spawner.SpawnEnemy(_enemyType.ToString(), GetRandomPosition());
            }
            else
            {
                Debug.LogWarning("EnemySpawner not found in the scene.");
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        float radius = _spawnRangeCollider.radius;
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        return transform.position + new Vector3(randomPoint.x, randomPoint.y, 0f);
    }

    private float GetRandomInterval()
    {
        return Random.Range(spawnInterval * 0.5f, spawnInterval * 2f);
    }   
}

