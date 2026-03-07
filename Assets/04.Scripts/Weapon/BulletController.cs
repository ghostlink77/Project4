using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BulletSoundController))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class BulletController : MonoBehaviour
{
    #region 이벤트
    public event Action OnHit;
    public void InvokeOnHit() => OnHit?.Invoke();
    #endregion

    #region 투사체 스탯
    private float _projectileSpeed;
    private float _projectileDmg;
    public float ProjectileDmg { get => _projectileDmg; }
    #endregion
    
    private TrailRenderer _trailRenderer;
    private Coroutine _deactivateCoroutine;

    [SerializeField]
    [Header("총알 수명(초)")]
    private float _lifeTime = 3f;
    
    [Header("한번만 재생하고 삭제 여부")]
    [SerializeField]
    private bool _deleteAfterAnimation;

    [Header("총알 관통 여부")]
    [SerializeField]
    private bool _penetratable = false;
    public bool Penetratable { get => _penetratable; set => _penetratable = value; }

    [Header("폭발형 투사체 여부 (직접 데미지 스킵)")]
    [SerializeField]
    private bool _isExplosive = false;
    public bool IsExplosive { get => _isExplosive; set => _isExplosive = value; }

    public bool DeleteAfterAnimation { get => _deleteAfterAnimation; set => _deleteAfterAnimation = value; }

    #region 오브젝트 풀링
    private IObjectPool<GameObject> _projectilePool;
    public IObjectPool<GameObject> ProjectilePool { get => _projectilePool; set => _projectilePool = value; }
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

        _deactivateCoroutine = StartCoroutine(DeactivateAfterTime());
    }
    private void OnDisable()
    {
        StopCoroutine(_deactivateCoroutine);
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
        if (_trailRenderer == null) return;
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
            // NOTE: 폭발형 투사체는 직접 데미지를 주지 않고 GenerateExplosion에서 범위 데미지를 처리한다
            if (!_isExplosive)
            {
                if (other.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(_projectileDmg);
                }
            }
            OnHit?.Invoke();
            if (!_penetratable) Release();
        }
    }
}
