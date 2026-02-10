using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;

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

    private void LoadExpObjectPrefab()
    {
        _expObjectPrefab = Addressables.LoadAssetAsync<GameObject>("Prefabs/Item_Grounded/ExpObject").WaitForCompletion();
    }

    private void Start()
    {
        _expObjectPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(_expObjectPrefab),
                actionOnGet: Activateexp,
                actionOnRelease: Disableexp,
                actionOnDestroy: Destroyexp,
                collectionCheck: false,
                defaultCapacity: InitSize,
                maxSize: MaxSize
                );
    }

    public GameObject SpawnExpObject(Vector3 position)
    {
        GameObject expObject = _expObjectPool.Get();
        expObject.transform.position = position;
        return expObject;
    }
    public void ReturunToPool(GameObject expObject)
    {
        _expObjectPool.Release(expObject);
    }

    private void Activateexp(GameObject obj)
    {
        obj.SetActive(true);
    }
    private void Disableexp(GameObject obj)
    {
        obj.SetActive(false);
    }
    private void Destroyexp(GameObject obj)
    {
        Destroy(obj);
    }
}
