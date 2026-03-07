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

    private Rigidbody2D _agitRigidbody;

    private List<Enemy> _activedEnemies = new List<Enemy>();

    [SerializeField] private AudioClip _enemyAllDeadSound;

    protected override void Init()
    {
        base.Init();

        LoadEnemyPrefabs();
        _agitRigidbody = GameObject.FindGameObjectWithTag("Agit").GetComponent<Rigidbody2D>();
    }

    private async void LoadEnemyPrefabs()
    {
        AsyncOperationHandle<IList<GameObject>> handle =
            Addressables.LoadAssetsAsync<GameObject>("EnemyPrefabs", null);

        await handle.Task;

        Debug.Log($"적 프리팹 로드 완료: {handle.Result.Count}개");
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
        InGameManager.Instance.InGameUIController.AddTracedEnemyInMinimap(enemy.transform);

        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.Initialize(_agitRigidbody);
        _activedEnemies.Add(enemyComponent);
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
        InGameManager.Instance.InGameUIController.RemoveTracedEnemyInMinimap(enemy.transform);
        _activedEnemies.Remove(enemy.GetComponent<Enemy>());
    }


    // NOTE: 오브젝트 풀 콜백 메서드들

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

    public void DestroyAll()
    {
        foreach (var enemy in _activedEnemies)
        {
            enemy.TakeDamage(1000000000000);
            SoundManager.Instance?.PlaySFX(SoundType.Enemy, _enemyAllDeadSound);
        }
    }
}
