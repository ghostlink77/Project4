using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    // 사운드 에셋들
    [Header("발사음")]
    [SerializeField]
    private WeaponSoundData weaponSounds;

    private float _projectileSpeed;
    public float ProjectileSpeed {get => _projectileSpeed; set => _projectileSpeed = value;}
    
    private AudioSource audioSource;

    [SerializeField]
    private float _lifeTime = 3f;

    private IObjectPool<GameObject> _projectilePool;

    public void SetProjectilePool(IObjectPool<GameObject> pool) => _projectilePool = pool;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 투사체가 발사 시작되었을 때 출력할 코드들
    void OnEnable()
    {
        // 게임 오브젝트 활성화
        if (gameObject.activeSelf == false) gameObject.SetActive(true);
        StartCoroutine(DeactivateAfterTime());
        // 총알 사운드 출력
        PlaySound(weaponSounds.ShootSound);
    }
    
    void Update()
    {
        transform.Translate(Vector2.right * _projectileSpeed * Time.deltaTime);
    }
    
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
            Debug.Log("트리거 감지");
            ReturnToPool();
        }
    }

    // 총알 사운드 출력하는 메서드
    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        if (CheckAbleToShoot(audioSource.clip) == false) return;
        // Debug.Log($"{audioSource.clip.name} 파일 재생");
        audioSource.Play();
    }
    
    // 사격이 가능한지 확인하는 메서드
    // 사격이 가능하면 true 반환
    private bool CheckAbleToShoot(AudioClip clip)
    {
        // 사운드 클립이 존재하는지 확인
        if (clip == null)
        {
            Debug.LogError($"AudioClip이 존재하지 않음");
            return false;
        }
        return true;
    }
}
