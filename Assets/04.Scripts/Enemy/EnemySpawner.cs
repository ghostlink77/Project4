/*
 * 적 스폰 및 오브젝트 풀 관리
 * 스폰된 적의 Transform 정보 유지
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;

public class EnemySpawner : SingletonBehaviour<EnemySpawner>
{
    private const int MaxSize = 50;
    private const int InitSize = 10;

    private Dictionary<string, ObjectPool<GameObject>> _enemyPools = new Dictionary<string, ObjectPool<GameObject>>();
    private Dictionary<string, GameObject> _enemyPrefabs = new Dictionary<string, GameObject>();

    private List<Transform> _activeEnemies = new List<Transform>();

    protected override void Init()
    {
        base.Init();

        LoadEnemyPrefabs();
        CreatePools();
    }

    private void LoadEnemyPrefabs()
    {
        GameObject[] prefabs = Addressables.LoadAssetAsync<GameObject[]>("EnemyPrefabs").WaitForCompletion();

        foreach (var prefab in prefabs)
        {
            _enemyPrefabs[prefab.name] = prefab;
        }
    }
    private void CreatePools()
    {
        foreach (var kvp in _enemyPrefabs)
        {
            string enemyType = kvp.Key;
            Debug.Log($"적 프리팹 로드: {enemyType}");
            GameObject prefab = kvp.Value;

            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
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
        enemyComponent.Initialize();
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

    // 오브젝트 풀 콜백 메서드들

    private void ActivateEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
        _activeEnemies.Add(enemy.transform);
        Debug.Log($"적 활성화. 현재 활성 적 수: {_activeEnemies.Count}");
    }

    private void DisableEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        if(_activeEnemies.Contains(enemy.transform))
        {
            _activeEnemies.Remove(enemy.transform);
        }
    }

    private void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
}
