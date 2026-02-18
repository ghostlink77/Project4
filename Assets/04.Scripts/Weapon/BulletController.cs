using System;
using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    #region 이벤트
    public event Action Generate, Hit, Remove;
    public void EventInvoke(Action eventName) => eventName?.Invoke();
    #endregion

    [Header("애니메이션 클립(등록 전 툴팁 읽어주세요)")]
    [Tooltip("애니메이션을 한번만 출력하고 없앨 경우에만 등록. 이외에는 등록할 필요 없음.")]
    [SerializeField]
    private AnimationClip _animationClip;

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
    SpriteRenderer _spriteRenderer;
    Collider2D _collider2D;
    BulletSoundController _bulletSoundController;
    WaitForSeconds _delayForBulletDisable;
    #warning 적 피격음만큼 기다리게 하는 WaitForSeconds()를 캐싱해두는 변수가 있었으나, 일단은 임시로 사운드매니저에서 출력을 담당하므로 쓸 일이 없어 숨겨둠. 이후에 총알 착탄지점에서 소리가 들리도록 만들려면 이것을 사용하도록 함.
    // WaitForSeconds _delayForHitSound;
    #endregion

    void Awake()
    {
        CashingComponents();
        _bulletSoundController = GetComponent<BulletSoundController>();
        SetDeleteTime();
        CashingWaitForSeconds();
    }

    #region 캐싱 메서드
    // 각종 컴포넌트들을 캐싱하는 메서드들
    private void CashingComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
    }

    private void CashingWaitForSeconds()
    {
        _delayForBulletDisable = new WaitForSeconds(_lifeTime);
        // if (_bulletSoundController.HitSound != null) _delayForHitSound = new WaitForSeconds(_bulletSoundController.HitSound.length);
    }
    #endregion

    #region 유니티 생명주기 메서드
    // 투사체가 발사 시작되었을 때 출력할 코드들
    void OnEnable()
    {
        _spriteRenderer.enabled = true;
        _collider2D.enabled = true;

        StartCoroutine(DeactivateAfterTime());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        _spriteRenderer.enabled = true;
        _collider2D.enabled = true;
    }

    void Update()
    {
        transform.Translate(Vector2.right * _projectileSpeed * Time.deltaTime);
    }
    #endregion

    #region 스탯 설정 메서드
    // 투사체 데미지 받아오는 스크립트
    public void SetDmg(int dmg) => _projectileDmg = dmg;
    public void SetProjectileSpeed(float projSpeed) => _projectileSpeed = projSpeed;
    #endregion
    
    #region 코루틴 함수
    private IEnumerator DeactivateAfterTime()
    {
        yield return _delayForBulletDisable;
        ReturnToPool();
        
    }
    
    #warning 임시방편으로 사운드매니저를 사용하기 때문에 착탄지점에서 완전히 비활성화하지 않고 남겨둘 필요가 없으므로 주석처리해둠
    /*
    private IEnumerator DelayedRelease()
    {
        _spriteRenderer.enabled = false;
        _collider2D.enabled = false;
        
        // yield return new WaitWhile(() => SoundManager.Instance.GetAudioSource(SoundType.Enemy).isPlaying);
        // yield return _delayForHitSound;
        
        _projectilePool.Release(gameObject);
    }
    */
    #endregion
    
    #warning 코루틴을 사용하지 않으므로 임시로 추가한 오브젝트 오브젝트 풀링으로 총알 넣는 함수
    private void ImmediateRelease()
    {
        _spriteRenderer.enabled = false;
        _collider2D.enabled = false;
        _projectilePool.Release(gameObject);
    }
    
    void ReturnToPool()
    {
        if (gameObject.activeSelf) _projectilePool.Release(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!gameObject.activeSelf) return;
            // StartCoroutine(DelayedRelease());
            ImmediateRelease();
            
            other.GetComponent<IDamageable>().TakeDamage(_projectileDmg);
            EventInvoke(Hit);
        }
    }
    
    // 애니메이션이 사라지는 시간을 결정하는 함수
    private void SetDeleteTime()
    {
        if (!_deleteAfterAnimation) return;
        if (_animationClip != null) _lifeTime = _animationClip.length;
    }
}