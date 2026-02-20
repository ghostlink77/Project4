// NOTE: 개별 스폰 포인트에서 적을 주기적으로 생성하는 스크립트
using System.Collections;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType = EnemyType.Drone1;

    [SerializeField] private float _spawnInterval = 1f;
    public float SpawnInterval { get => _spawnInterval; set => _spawnInterval = value; }

    private bool _isSpawning = false;
    private Coroutine _spawnCoroutine;

    private CircleCollider2D _spawnRangeCollider;

    private void Awake()
    {
        _spawnRangeCollider = GetComponentInChildren<CircleCollider2D>();
    }

    public void StartSpawn(EnemyType enemyType)
    {
        _isSpawning = true;
        _enemyType = enemyType;
        _spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawn()
    {
        if (!_isSpawning || _spawnCoroutine == null)
        {
            return;
        }
        _isSpawning = false;
        StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = null;
    }

    private IEnumerator SpawnLoop()
    {
        while (_isSpawning)
        {
            yield return new WaitForSeconds(GetRandomInterval());
            if (EnemySpawner.Instance != null)
            {
                EnemySpawner.Instance.SpawnEnemy(_enemyType.ToString(), GetRandomPosition());
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
        return Random.Range(_spawnInterval * 0.5f, _spawnInterval * 2f);
    }
}
