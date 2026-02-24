using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.WSA;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class EffectController : MonoBehaviour
{
    public event Action OnHit;

    private Coroutine _releaseCoroutine;
    private AudioSource _audioSource;
    private Collider2D _collider;
    private AnimationClip _explosionAnimationClip;
    private Animator _animator;

    [Header("Settings")]
    [SerializeField] private float _lifeTime = 1f;
    [SerializeField] private bool _isProjectile = false;
    private bool _alreadyDamaged = false;
    
    private int _damage;
    private WaitForSeconds _waitLifeTime;
    private IObjectPool<GameObject> _pool;
    private readonly List<Collider2D> _overlapResults = new List<Collider2D>();
    private ContactFilter2D _contactFilter;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        
        _contactFilter.useTriggers = true;
        _contactFilter.useLayerMask = true;
    }

    private void OnEnable()
    {
        _collider.enabled = true;
        _alreadyDamaged = false;
        
        float length = _animator.GetCurrentAnimatorStateInfo(0).length;
        OnHit += DisableColliderOnHit;
        _releaseCoroutine = StartCoroutine(ReleaseAfterTime(length));
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
        _damage = damage;
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    private IEnumerator ReleaseAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
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
        
        // Unity 6 deprecation fix: OverlapCollider -> Overlap
        int hitCount = _collider.Overlap(_contactFilter, _overlapResults);
        
        if (hitCount > 0)
        {
            foreach (var hit in _overlapResults)
            {
                if (hit.CompareTag("Enemy"))
                {
                    if (hit.TryGetComponent<IDamageable>(out var target))
                    {
                        target.TakeDamage(_damage);
                    }
                }
                else if (hit.CompareTag("Player"))
                {
                    if (hit.TryGetComponent<PlayerStatController>(out var player))
                    {
                        int reducedDamage = (int)(_damage * 0.6);
                        player.TakeDamage(reducedDamage);
                    }
                }
            }
            OnHit?.Invoke();
        }
    }
}
