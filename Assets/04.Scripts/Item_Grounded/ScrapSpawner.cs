// NOTE: 고철 아이템 풀링, 스폰 및 맵 내 랜덤 배치 관리
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class ScrapSpawner : MonoBehaviour
{
    private const int MaxSize = 30;
    private const int InitSize = 10;
    private const float SpawnInterval = 1f;

    [SerializeField] private int _maxActiveScrap = 20;
    [SerializeField] private float _scrapLifetime = 30f;

    private ObjectPool<GameObject> _scrapPool;
    private GameObject _scrapPrefab;
    private int _activeScrapCount;

    [SerializeField] private BoxCollider2D _mapCollider;
    private Bounds _mapBound;

    private bool _isSpawning = false;
    private WaitForSeconds _waitSpawnInterval = new WaitForSeconds(SpawnInterval);

    private void Awake()
    {
        LoadScrapPrefab();
        if (_mapCollider)
        {
            _mapCollider.gameObject.SetActive(true);
            _mapBound = _mapCollider.bounds;
            _mapCollider.gameObject.SetActive(false);
        }
    }

    private async void LoadScrapPrefab()
    {
        AsyncOperationHandle<GameObject> handle =
            Addressables.LoadAssetAsync<GameObject>("Prefabs/Item_Grounded/Scrap");
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _scrapPrefab = handle.Result;
        }
        else
        {
            Debug.LogError("Failed to load Scrap prefab.");
        }
        CreatePool();
    }

    private void CreatePool()
    {
        _scrapPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(_scrapPrefab),
                actionOnGet: ActivateScrap,
                actionOnRelease: DisableScrap,
                actionOnDestroy: DestroyScrap,
                collectionCheck: false,
                defaultCapacity: InitSize,
                maxSize: MaxSize
                );

        StartSpawn();
    }

    public GameObject SpawnScrap(Vector3 position)
    {
        if (_activeScrapCount >= _maxActiveScrap) return null;

        GameObject scrap = _scrapPool.Get();
        scrap.transform.position = position;
        Scrap scrapComponent = scrap.GetComponent<Scrap>();
        scrapComponent.Initialize(this, _scrapLifetime);
        _activeScrapCount++;
        return scrap;
    }

    public void StartSpawn()
    {
        _isSpawning = true;
        StartCoroutine(SpawnScrapLoop());
    }

    public void StopSpawn()
    {
        _isSpawning = false;
        StopCoroutine("SpawnScrapLoop");
    }

    private IEnumerator SpawnScrapLoop()
    {
        while (true)
        {
            if (_isSpawning && _activeScrapCount < _maxActiveScrap)
            {
                SpawnScrap(GetRandomPosition());
            }
            yield return _waitSpawnInterval;
        }
    }

    private Vector3 GetRandomPosition()
    {
        var randomPosition = new Vector3(
            Random.Range(_mapBound.min.x, _mapBound.max.x),
            Random.Range(_mapBound.min.y, _mapBound.max.y),
            0f
        );

        return randomPosition;
    }

    public void ReturnToPool(GameObject scrap)
    {
        _activeScrapCount--;
        _scrapPool.Release(scrap);
    }

    private void ActivateScrap(GameObject scrap)
    {
        scrap.SetActive(true);
    }

    private void DisableScrap(GameObject scrap)
    {
        scrap.SetActive(false);
    }

    private void DestroyScrap(GameObject scrap)
    {
        Destroy(scrap);
    }
}
