using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class Minimap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider2D _mapReference;
    [SerializeField] private Transform _playerTransform;

    [Header("References - UI")]
    [SerializeField] private RectTransform _playerIndicator;
    [SerializeField] private GameObject _enemyIndicatorPrefab;
    [SerializeField] private RectTransform MinimapContainer;

    [Header("Object Pool Settings")]
    [SerializeField] private int _initSize;
    [SerializeField] private int _maxSize;
    private IObjectPool<RectTransform> _enemyIndicatorPool;
    private Dictionary<Transform, RectTransform> _enemyIndicatorPools = new Dictionary<Transform, RectTransform>();
   
    [Header("Parameters")]
    [SerializeField] private Vector2 _mapTextureSize = new Vector2(1024, 1024);
    [SerializeField] private Bounds _mapBounds;

    [Header("Player Options")]
    [SerializeField] private float minimapScale = 1.0f;

    Vector2 _unitScale;
    Vector2 _mapPosition = Vector2.zero;

    private void Awake()
    {
        if (_mapReference)
        {
            _mapReference.gameObject.SetActive(true);
            _mapBounds = _mapReference.bounds;
            _mapReference.gameObject.SetActive(false);

            _unitScale = new Vector2(_mapTextureSize.x / _mapBounds.size.x,
            _mapTextureSize.y / _mapBounds.size.y);
        }

        CreatePools();
    }

    private void LateUpdate()
    {
        if (_playerTransform == null || _playerIndicator == null) return;

        Transform positionReference = _playerTransform;
        Vector3 positionOffset = _mapBounds.center - positionReference.position;

        _mapPosition.x = positionOffset.x * _unitScale.x * -1 * minimapScale;
        _mapPosition.y = positionOffset.y * _unitScale.y * -1 * minimapScale;

        _playerIndicator.localPosition = _mapPosition;

        ShowEnemyObject();

    }

    private void CreatePools()
    {
        RectTransform _enemyIndicatorObj = _enemyIndicatorPrefab.GetComponent<RectTransform>();
        var pool = new ObjectPool<RectTransform>(
            createFunc: () => Instantiate(_enemyIndicatorObj),
            actionOnGet: ActivateIndicator,
            actionOnRelease: DisableIndicator,
            collectionCheck: false,
            defaultCapacity: _initSize,
            maxSize: _maxSize);
        _enemyIndicatorPool = pool;
    }

    private void ActivateIndicator(RectTransform obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void DisableIndicator(RectTransform obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void ShowEnemyObject()
    {
        if (_enemyIndicatorPool == null || _enemyIndicatorPrefab == null) return;
        
        foreach(var tracedEnemy in _enemyIndicatorPools)
        {
            Transform positionReference = tracedEnemy.Key;
            Vector3 positionOffset = _mapBounds.center - positionReference.position;

            float x = positionOffset.x * _unitScale.x * -1 * minimapScale;
            float y = positionOffset.y * _unitScale.y * -1 * minimapScale;

            tracedEnemy.Value.localPosition = new Vector2(x, y);
        }
        
    }

    public void AddTracedEnemy(Transform transform)
    {
        if (_enemyIndicatorPools.ContainsKey(transform)) return;
        var enemyIndicator = _enemyIndicatorPool.Get();
        enemyIndicator.SetParent(MinimapContainer, false);
        _enemyIndicatorPools[transform] = enemyIndicator;
    }

    public void RemoveTracedEnemy(Transform transform)
    {
        if (_enemyIndicatorPools.TryGetValue(transform, out var indicator))
        {
            _enemyIndicatorPool.Release(indicator);
            _enemyIndicatorPools.Remove(transform);
        }
    }
}
