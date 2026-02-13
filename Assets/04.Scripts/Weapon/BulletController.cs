using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    #region 사운드 에셋들
    [Header("발사음")]
    [SerializeField]
    private AudioClip _shootSound;
    #endregion

    private float _projectileSpeed;
    private int _projectileDmg;
    
    private AudioSource _audioSource;

    [SerializeField]
    private float _lifeTime = 3f;

    private IObjectPool<GameObject> _projectilePool;

    public void SetProjectilePool(IObjectPool<GameObject> pool) => _projectilePool = pool;

    void Awake()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    // 투사체가 발사 시작되었을 때 출력할 코드들
    void OnEnable()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        StartCoroutine(DeactivateAfterTime());

        PlaySound(_shootSound);
    }

    void OnDisable()
    {
        StopAllCoroutines();
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    void Update()
    {
        transform.Translate(Vector2.right * _projectileSpeed * Time.deltaTime);
    }
    
    // 투사체 데미지 받아오는 스크립트
    public void SetDmg(int dmg) => _projectileDmg = dmg;
    public void SetProjectileSpeed(float projSpeed) => _projectileSpeed = projSpeed;
    
    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        ReturnToPool();
        
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
            StartCoroutine(DelayedRelease());
            other.GetComponent<IDamageable>().TakeDamage(_projectileDmg);
        }
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
            Debug.LogError($"오디오클립이 null임");
            return true;
        }
        return false;
    }
    // 오디오 소스가 null인지 확인하는 메서드
    private bool NullAudioSource()
    {
        if (_audioSource == null)
        {
            Debug.LogError("오디오소스가 null임");
            return true;
        }
        return false;
    }
    
    #endregion
}
