/*
 * 적 스폰 및 오브젝트 풀 관리
 * 스폰된 적의 Transform 정보 유지
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class EnemySpawner : SingletonBehaviour<EnemySpawner>
{
    private const int MaxSize = 50;
    private const int InitSize = 10;

    private Dictionary<string, ObjectPool<GameObject>> _enemyPools = new Dictionary<string, ObjectPool<GameObject>>();
    private Dictionary<string, GameObject> _enemyPrefabs = new Dictionary<string, GameObject>();

    private List<Transform> _activeEnemies = new List<Transform>();

    private Rigidbody2D _agitRigidbody;

    protected override void Init()
    {
        base.Init();

        LoadEnemyPrefabs();
        _agitRigidbody = GameObject.FindGameObjectWithTag("Agit").GetComponent<Rigidbody2D>();
    }

    private async void LoadEnemyPrefabs()
    {
        AsyncOperationHandle<GameObject[]> handle =
            Addressables.LoadAssetAsync<GameObject[]>("EnemyPrefabs");

        await handle.Task;

        foreach (var prefab in handle.Result)
        {
            _enemyPrefabs[prefab.name] = prefab;
        }

        CreatePools();
    }

    private void CreatePools()
    {
        foreach (var kvp in _enemyPrefabs)
        {
            string enemyType = kvp.Key;
            Debug.Log($"적 프리팹 로드: {enemyType}");
            GameObject prefab = kvp.Value;

            var pool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefab),
                actionOnGet: ActivateEnemy,
                actionOnRelease: DisableEnemy,
                actionOnDestroy: DestroyEnemy,
                collectionCheck: false,
                defaultCapacity: InitSize,
                maxSize: MaxSize);
            _enemyPools.Add(enemyType, pool);
        }
        Debug.Log($"적 풀 {_enemyPools.Count}개 생성 완료");
    }

    public GameObject SpawnEnemy(string enemyType, Vector3 position)
    {
        if (!_enemyPrefabs.ContainsKey(enemyType))
        {
            Debug.LogError($"Enemy type '{enemyType}' not found!");
            return null;
        }
        GameObject enemy = _enemyPools[enemyType].Get();
        enemy.transform.position = position;

        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.Initialize(_agitRigidbody);
        return enemy;
    }

    public void ReturnToPool(string enemyType, GameObject enemy)
    {
        if (_enemyPools.ContainsKey(enemyType))
        {
            _enemyPools[enemyType].Release(enemy);
        }
        else
        {
            Destroy(enemy);
        }
    }

    public List<Transform> GetActiveEnemies()
    {
        return _activeEnemies;
    }

    // NOTE: 오브젝트 풀 콜백 메서드들

    private void ActivateEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
        _activeEnemies.Add(enemy.transform);
        Debug.Log($"적 활성화. 현재 활성 적 수: {_activeEnemies.Count}");
    }

    private void DisableEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        if (_activeEnemies.Contains(enemy.transform))
        {
            _activeEnemies.Remove(enemy.transform);
        }
    }

    private void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
}
