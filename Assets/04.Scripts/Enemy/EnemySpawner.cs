using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    private ObjectPool<GameObject> enemyPool;
    private const int MAXSIZE = 50;
    private const int INITSIZE = 10;

    // 적 오브젝트 종류별로 오브젝트 풀 관리
    private Dictionary<string, ObjectPool<GameObject>> enemyPools = new Dictionary<string, ObjectPool<GameObject>>();
    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        LoadEnemyPrefabs();
        CreatePools();
    }

    private void LoadEnemyPrefabs()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies");

        foreach (var prefab in prefabs)
        {
            enemyPrefabs[prefab.name] = prefab;
        }
    }
    private void CreatePools()
    {
        foreach (var kvp in enemyPrefabs)
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
                defaultCapacity: INITSIZE,
                maxSize: MAXSIZE);
            enemyPools.Add(enemyType, pool);
        }
        Debug.Log($"적 풀 {enemyPools.Count}개 생성 완료");
    }


    public GameObject SpawnEnemy(string enemyType, Vector3 position)
    {
        if (!enemyPrefabs.ContainsKey(enemyType))
        {
            Debug.LogError($"Enemy type '{enemyType}' not found!");
            return null;
        }
        GameObject enemy = enemyPools[enemyType].Get();
        enemy.transform.position = position;
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.Initialize();
        return enemy;
    }

    public void ReturnToPool(string enemyType, GameObject enemy)
    {
        if (enemyPools.ContainsKey(enemyType))
        {     
            enemyPools[enemyType].Release(enemy);
        }
        else
        {
            Destroy(enemy);
        }
    }

    // 오브젝트 풀 콜백 메서드들

    private void ActivateEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
    }

    private void DisableEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    private void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
}
