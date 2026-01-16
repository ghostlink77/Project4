using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    // 사운드 에셋들
    [Header("발사음")]
    [SerializeField]
    private SoundData shootingSounds;
    
    [Header("발사음 리스트 번호")]
    [SerializeField]
    private int shootSoundListNumber;
    
    // 오디오 소스
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
        PlayBulletSound();
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
    private void PlayBulletSound()
    {
        audioSource.clip = shootingSounds.GetClip(shootSoundListNumber);
        if (CheckAbleToShoot(shootingSounds, shootSoundListNumber) == false)
        {
            Debug.LogError("사운드 파일 재생 불가");
            return;
        }
        // Debug.Log($"{audioSource.clip.name} 파일 재생");
        audioSource.Play();
    }
    
    // 사격이 가능한지 확인하는 메서드
    // 사격이 가능하면 true 반환
    private bool CheckAbleToShoot(SoundData scriptableObject, int clipNumber)
    {
        // 사운드 클립이 존재하는지 확인
        if (scriptableObject.GetClip(clipNumber) == null)
        {
            Debug.LogError($"{scriptableObject.name}의 {clipNumber}번 항목이 존재하지 않음");
            return false;
        }
        // 사운드 클립이 일치하는지 확인
        else if (audioSource.clip != scriptableObject.GetClip(clipNumber))
        {
            Debug.LogError($"필요한 사운드 클립: {scriptableObject.GetClip(clipNumber)}\n현재 할당된 사운드 클립: {audioSource.clip.name}");
        }
        return true;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
