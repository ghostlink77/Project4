using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    // 사운드 에셋들
    [Header("발사음")]
    [SerializeField]
    private WeaponSoundData weaponSounds;
    
    // 오디오 실행할 오디오 소스(총알 프리팹에 존재)
    [SerializeField]
    private AudioSource audioSource;
    
    // 오브젝트 위치
    // 플레이어 게임 오브젝트
    private GameObject playerObj;
    // 적 게임 오브젝트
    private GameObject enemyObj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        // 게임 오브젝트 비활성화
        if (gameObject.activeSelf == true) gameObject.SetActive(false);
    }

    // 투사체가 발사 시작되었을 때 출력할 코드들
    void OnEnable()
    {
        // 게임 오브젝트 활성화
        if (gameObject.activeSelf == false) gameObject.SetActive(true);
        // 총알 사운드 출력
        PlaySound(weaponSounds.ShootSound);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("트리거 감지");
            gameObject.SetActive(false);
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
    
    void Update()
    {
        
    }
}
