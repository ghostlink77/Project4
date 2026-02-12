using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ExpObjectSpawner : SingletonBehaviour<ExpObjectSpawner>
{
    private const int MaxSize = 100;
    private const int InitSize = 10;

    private ObjectPool<GameObject> _expObjectPool;
    private GameObject _expObjectPrefab;

    protected override void Init()
    {
        base.Init();
        LoadExpObjectPrefab();
    }

    private async void LoadExpObjectPrefab()
    {
        AsyncOperationHandle<GameObject> handle =
            Addressables.LoadAssetAsync<GameObject>("Prefabs/Item_Grounded/ExpObject");

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _expObjectPrefab = handle.Result;
        }
        else
        {
            Debug.LogError("Failed to load ExpObject prefab.");
        }

        CreatePool();
    }

    private void CreatePool()
    {
        _expObjectPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(_expObjectPrefab),
                actionOnGet: ActivateExpObject,
                actionOnRelease: DisableExpObject,
                actionOnDestroy: DestroyExpObject,
                collectionCheck: false,
                defaultCapacity: InitSize,
                maxSize: MaxSize
                );
    }

    public GameObject SpawnExpObject(Vector3 position)
    {
        GameObject expObject = _expObjectPool.Get();
        expObject.transform.position = position;
        ExpObject expObjComponent = expObject.GetComponent<ExpObject>();
        expObjComponent.Initialize();
        return expObject;
    }
    public void ReturnToPool(GameObject expObject)
    {
        _expObjectPool.Release(expObject);
    }

    private void ActivateExpObject(GameObject obj)
    {
        obj.SetActive(true);
    }
    private void DisableExpObject(GameObject obj)
    {
        obj.SetActive(false);
    }
    private void DestroyExpObject(GameObject obj)
    {
        Destroy(obj);
    }
}
