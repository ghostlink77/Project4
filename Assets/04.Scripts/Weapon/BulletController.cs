using System;
using System.Collections;
using System.ComponentModel;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UIElements;

[RequireComponent(typeof(BulletSoundController))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class BulletController : MonoBehaviour
{
    #region 이벤트
    public event Action<Collider2D> OnHit;
    public void InvokeOnHit(Collider2D hitTarget) => OnHit?.Invoke(hitTarget);
    #endregion

    #region 투사체 스탯
    private float _projectileSpeed;
    private float _projectileDmg;
    public float ProjectileDmg {get => _projectileDmg;}
    #endregion
    private TrailRenderer _trailRenderer;
    
    [SerializeField]
    [Header("총알 수명(초)")]
    private float _lifeTime = 3f;
    
    [Header("한번만 재생하고 삭제 여부")]
    [SerializeField]
    private bool _deleteAfterAnimation;

    [Header("총알 관통 여부")]
    [SerializeField]
    private bool _penetratable = false;
    public bool Penetratable {get => _penetratable; set => _penetratable = value;}

    public bool DeleteAfterAnimation {get => _deleteAfterAnimation; set => _deleteAfterAnimation = value;}

    #region 오브젝트 풀링
    private IObjectPool<GameObject> _projectilePool;
    public IObjectPool<GameObject> ProjectilePool {get => _projectilePool; set => _projectilePool = value;}
    #endregion
    
    #region 참조변수
    private WaitForSeconds _delayForBulletDisable;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    #endregion

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        CashingWaitForSeconds();
    }

    #region 캐싱 메서드
    private void CashingWaitForSeconds()
    {
        _delayForBulletDisable = new WaitForSeconds(_lifeTime);
    }
    #endregion

    #region 유니티 생명주기 메서드
    private void OnEnable()
    {
        _spriteRenderer.enabled = true;
        _collider2D.enabled = true;
        ResetTrailRendererLine();

        StartCoroutine(DeactivateAfterTime());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update() => transform.Translate(Vector2.right * _projectileSpeed * Time.deltaTime);
    #endregion

    #region 스탯 설정 및 반환 메서드
    public void GetNeededVariableForAttack(float dmg, float speed, IObjectPool<GameObject> pool, WeaponEventController eventController)
    {
        _projectileDmg = dmg;
        _projectileSpeed = speed;
        _projectilePool = pool;
    }
    
    private void ResetTrailRendererLine()
    {
        if(_trailRenderer == null) return;
        _trailRenderer.Clear();
    }
    
    public void SetLifeTime(float lifeTime) => _delayForBulletDisable = new WaitForSeconds(lifeTime);
    #endregion
    
    #region 코루틴 함수
    private IEnumerator DeactivateAfterTime()
    {
        yield return _delayForBulletDisable;
        Release();
    }
    #endregion
    
    private void Release()
    {
        if (gameObject.activeSelf) _projectilePool.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && gameObject.activeSelf)
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(_projectileDmg);
            }
            InvokeOnHit(other);
            if (!Penetratable) Release();
        }
    }
}