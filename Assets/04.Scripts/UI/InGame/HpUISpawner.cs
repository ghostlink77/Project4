using UnityEngine;
using UnityEngine.Pool;

public class HpUISpawner : MonoBehaviour
{
    private IObjectPool<HpUI> _hpUIPool;
    [SerializeField] private GameObject _hpUIPrefab;

    [SerializeField] private int _initSize = 10;
    [SerializeField] private int _maxSize = 50;

    [SerializeField] private Transform _spawnParent;
    private void Awake()
    {
        CreatePools();
    }

    private void CreatePools()
    {
        HpUI HpUIObj = _hpUIPrefab.GetComponent<HpUI>();
        var pool = new ObjectPool<HpUI>(
            createFunc: () => Instantiate(HpUIObj),
            actionOnGet: ActivateUi,
            actionOnRelease: DisableUi,
            collectionCheck: false,
            defaultCapacity: _initSize,
            maxSize: _maxSize);
        _hpUIPool = pool;
    }

    private void ActivateUi(HpUI obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void DisableUi(HpUI obj)
    {
        obj.gameObject.SetActive(false);
    }

    public HpUI CreateHpUI(Vector3 position)
    {
        if (_hpUIPool == null)
        {
            Debug.Log("Hp UI Pools is nothing.");
            return null;
        }

        var hpUI = _hpUIPool.Get();
        if (_spawnParent != null)
            hpUI.transform.SetParent(_spawnParent, false);
        hpUI.SetPosition(position);
        
        return hpUI;
    }

    public void RemoveHpUI(HpUI obj)
    {
        _hpUIPool.Release(obj);
    }
}
