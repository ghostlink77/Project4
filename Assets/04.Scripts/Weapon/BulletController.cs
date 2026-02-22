using System;
using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    #region 이벤트
    public event Action Generate, Hit, Remove;
    public void CallEventGenerate() => Generate?.Invoke();
    public void CallEventHit() => Hit?.Invoke();
    public void CallEventRemove() => Remove?.Invoke();
    #endregion

    #region 투사체 스탯
    private float _projectileSpeed;
    private int _projectileDmg;
    #endregion
    
    [SerializeField]
    [Header("총알 수명(초)")]
    private float _lifeTime = 3f;
    
    [Header("한번만 재생하고 삭제 여부")]
    [SerializeField]
    private bool _deleteAfterAnimation;
    public bool DeleteAfterAnimation {get => _deleteAfterAnimation; set => _deleteAfterAnimation = value;}

    #region 오브젝트 풀링
    private IObjectPool<GameObject> _projectilePool;
    public void SetProjectilePool(IObjectPool<GameObject> pool) => _projectilePool = pool;
    #endregion
    
    #region 참조변수
    WaitForSeconds _delayForBulletDisable;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    #endregion

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        CashingWaitForSeconds();
    }

    #region 캐싱 메서드
    private void CashingWaitForSeconds()
    {
        _delayForBulletDisable = new WaitForSeconds(_lifeTime);
    }
    #endregion

    #region 유니티 생명주기 메서드
    void OnEnable()
    {
        _spriteRenderer.enabled = true;
        _collider2D.enabled = true;

        CallEventGenerate();
        StartCoroutine(DeactivateAfterTime());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        CallEventRemove();
    }

    void Update() => transform.Translate(Vector2.right * _projectileSpeed * Time.deltaTime);
    #endregion

    #region 스탯 설정 및 반환 메서드
    public void SetUp(int dmg, float speed, IObjectPool<GameObject> pool)
    {
        _projectileDmg = dmg;
        _projectileSpeed = speed;
        _projectilePool = pool;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && gameObject.activeSelf)
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(_projectileDmg);
            }
            Hit?.Invoke();
            Release();
        }
    }
}