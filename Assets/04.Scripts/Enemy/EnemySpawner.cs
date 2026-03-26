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

    private List<Enemy> _activeEnemies = new List<Enemy>();

    [SerializeField] private AudioClip _enemyAllDeadSound;

    protected override void Init()
    {
        base.Init();

        LoadEnemyPrefabs();

        GameObject[] agitObjects = GameObject.FindGameObjectsWithTag("Agit");
        foreach (GameObject agitObject in agitObjects)
        {
            Rigidbody2D rb = agitObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                _agitRigidbody = rb;
                break;
            }
        }

        if (_agitRigidbody == null)
        {
        }
    }

    private async void LoadEnemyPrefabs()
    {
        AsyncOperationHandle<IList<GameObject>> handle =
            Addressables.LoadAssetsAsync<GameObject>("EnemyPrefabs", null);

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
    }

    public GameObject SpawnEnemy(string enemyType, Vector3 position)
    {
        if (!_enemyPrefabs.ContainsKey(enemyType))
        {
            return null;
        }

        GameObject enemy = _enemyPools[enemyType].Get();
        enemy.transform.position = position;
        InGameManager.Instance.InGameUIController.AddTracedEnemyInMinimap(enemy.transform);

        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.Initialize(_agitRigidbody);
        _activeEnemies.Add(enemyComponent);
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
        _activeEnemies.Remove(enemy.GetComponent<Enemy>());
    }



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
        foreach (var enemy in _activeEnemies)
        {
            enemy.TakeDamage(Mathf.Infinity);
        }
        SoundManager.Instance?.PlaySFX(SoundType.Enemy, _enemyAllDeadSound);
    }
}
