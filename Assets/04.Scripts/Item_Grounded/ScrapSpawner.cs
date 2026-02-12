using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class ScrapSpawner : MonoBehaviour
{
    private const int MaxSize = 30;
    private const int InitSize = 10;

    private ObjectPool<GameObject> _scrapPool;
    private GameObject _scrapPrefab;

    [SerializeField] private BoxCollider2D _mapCollider;
    private Bounds _mapBound;

    private bool _isSpawning = false;

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
        GameObject scrap = _scrapPool.Get();
        scrap.transform.position = position;
        Scrap scrapComponent = scrap.GetComponent<Scrap>();
        scrapComponent.Initialize(this);
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
            // TODO: 스폰 조건, 위치, 간격 등 조정
            if(_isSpawning)
            {
                SpawnScrap(GetRandomPosition());
            }
            yield return new WaitForSeconds(5f);
        }
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(_mapBound.min.x, _mapBound.max.x),
            Random.Range(_mapBound.min.y, _mapBound.max.y),
            0f
        );

        return randomPosition;
    }

    public void ReturnToPool(GameObject scrap)
    {
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
