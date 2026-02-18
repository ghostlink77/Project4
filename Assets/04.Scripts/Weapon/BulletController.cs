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
    public event Action Hit;
    public void EventInvoke(Action eventName) => eventName?.Invoke();
    #endregion

    #region 사운드 에셋들
    [Header("발사음")]
    [SerializeField]
    private AudioClip _hitSound;
    #endregion
    [Header("애니메이션 클립(등록 전 툴팁 읽어주세요)")]
    [Tooltip("애니메이션을 한번만 출력하고 없앨 경우에만 등록. 이외에는 등록할 필요 없음.")]
    [SerializeField]
    private AnimationClip _animationClip;

    private float _projectileSpeed;
    private int _projectileDmg;
    
    private AudioSource _audioSource;

    [SerializeField]
    [Header("총알 수명(초)")]
    private float _lifeTime = 3f;

    private IObjectPool<GameObject> _projectilePool;

    public void SetProjectilePool(IObjectPool<GameObject> pool) => _projectilePool = pool;

    void Awake()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        SetDeleteTime();
    }

    #region 유니티 생명주기 메서드
    // 투사체가 발사 시작되었을 때 출력할 코드들
    void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        AddOnEvent();

        StartCoroutine(DeactivateAfterTime());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        RemoveFromEvent();
    }

    void Update()
    {
        transform.Translate(Vector2.right * _projectileSpeed * Time.deltaTime);
    }
    #endregion
    
    #region 이벤트 메서드
    private void AddOnEvent()
    {
        Hit += OnEventHit;
    }
    
    private void RemoveFromEvent()
    {
        Hit -= OnEventHit;
    }
    
    private void OnEventHit()
    {
        PlaySound(_hitSound);
    }
    #endregion

    // 투사체 데미지 받아오는 스크립트
    public void SetDmg(int dmg) => _projectileDmg = dmg;
    public void SetProjectileSpeed(float projSpeed) => _projectileSpeed = projSpeed;
    
    #region 코루틴 함수
    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        ReturnToPool();
        
    }
    
    private IEnumerator DelayedRelease()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        
        if (_audioSource != null && _audioSource.clip != null)
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
        }
        
        _projectilePool.Release(gameObject);
    }
    #endregion
    
    void ReturnToPool()
    {
        if (gameObject.activeSelf) _projectilePool.Release(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!gameObject.activeSelf) return;
            StartCoroutine(DelayedRelease());
            other.GetComponent<IDamageable>().TakeDamage(_projectileDmg);
            EventInvoke(Hit);
        }
    }
    
    // 애니메이션이 사라지는 시간을 결정하는 함수
    private void SetDeleteTime()
    {
        if (_animationClip != null) _lifeTime = _animationClip.length;
    }

    // 총알 사운드 출력하는 메서드
    private void PlaySound(AudioClip clip)
    {
        if (NullAudioClip(clip)) return;
        if (NullAudioSource()) return;
        if (_audioSource.clip != clip) _audioSource.clip = clip;
        _audioSource.Play();
    }
    
    #region null 점검 스크립트
    private bool NullAudioClip(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log($"오디오클립이 null임");
            return true;
        }
        return false;
    }
    // 오디오 소스가 null인지 확인하는 메서드
    private bool NullAudioSource()
    {
        if (_audioSource == null)
        {
            Debug.Log("오디오소스가 null임");
            return true;
        }
        return false;
    }
    
    #endregion
}