using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.WSA;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class EffectController : MonoBehaviour
{
    public event Action OnHit;

    private Coroutine _releaseCoroutine;
    private Collider2D _collider;
    private Animator _animator;

    [Header("Settings")]
    [SerializeField] private float _defaultLifeTime = 1f;
    [SerializeField] private bool _isProjectile = false;
    private bool _alreadyDamaged = false;
    
    private int _damage;
    private WaitForSeconds _waitForSecondsUntilDelete;
    private IObjectPool<GameObject> _pool;
    private readonly List<Collider2D> _overlapResults = new List<Collider2D>();
    private ContactFilter2D _contactFilter;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();

        _contactFilter.useTriggers = true;
        _contactFilter.useLayerMask = false;
    }

    private void OnEnable()
    {
        _collider.enabled = true;
        _alreadyDamaged = false;
        
        OnHit -= DisableColliderOnHit;
        OnHit += DisableColliderOnHit;
        
        _releaseCoroutine = StartCoroutine(ReleaseRoutine());
    }

    private void OnDisable()
    {
        if (_releaseCoroutine != null)
        {
            StopCoroutine(_releaseCoroutine);
            _releaseCoroutine = null;
        }
        OnHit -= DisableColliderOnHit;
    }

    private void DisableColliderOnHit()
    {
        _collider.enabled = false;
    }

    public void Init(int damage)
    {
        Debug.Log($"데미지 설정: {damage}");
        _damage = damage;
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    private IEnumerator ReleaseRoutine()
    {
        // 애니메이터가 상태를 완전히 인지할 때까지 한 프레임 대기하기 위해 사용
        yield return null;
        if (_waitForSecondsUntilDelete == null)
        {
            float length = _animator.GetCurrentAnimatorStateInfo(0).length;
            if (length <= 0) length = _defaultLifeTime;
            _waitForSecondsUntilDelete = new WaitForSeconds(length);
        }
        yield return _waitForSecondsUntilDelete;
        Release();
    }

    public void Release()
    {
        if (_pool != null && gameObject.activeSelf)
        {
            _pool.Release(gameObject);
        }
        else if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ApplyAreaDamageFromCollider();
        
        if (_isProjectile)
        {
            Release();
        }
    }
    
    private void ApplyAreaDamageFromCollider()
    {
        if (_alreadyDamaged) return;
        
        _overlapResults.Clear();
        
        int hitCount = _collider.Overlap(_contactFilter, _overlapResults);
        
        if (hitCount > 0)
        {
            foreach (var hit in _overlapResults)
            {
                if (hit.CompareTag("Enemy"))
                {
                    if (hit.TryGetComponent<IDamageable>(out var target))
                    {
                        Debug.Log($"적이 공격에 휘말림, damage: {_damage}");
                        target.TakeDamage(_damage);
                    }
                }
                if (hit.CompareTag("Player"))
                {
                    if (hit.TryGetComponent<PlayerStatController>(out var player))
                    {
                        int reducedDamage = (int)(_damage * 0.6);
                        Debug.Log($"플레이어가 폭발에 휘말림, damage: {reducedDamage}");
                        player.TakeDamage(reducedDamage);
                    }
                }
            }
            OnHit?.Invoke();
        }
    }
}
