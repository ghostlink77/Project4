using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    // 사운드 에셋들
    [Header("사운드 스크립터블 오브젝트")]
    [SerializeField]
    private SoundData shootingSounds;
    
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
    }

    // 투사체가 발사 시작되었을 때 출력할 코드들
    void OnEnable()
    {
        audioSource.clip = shootingSounds.GetClip(0);
        if (CheckAbleToShoot(shootingSounds, 0) == false) return;

        Debug.Log($"{audioSource.clip.name} 파일 재생");
        audioSource.Play();
    }
    
    // 사격이 가능한지 확인하는 메서드
    // 사격이 가능하면 true 반환
    private bool CheckAbleToShoot(SoundData scriptableObject, int clipNumber)
    {
        // 사운드 클립이 존재하는지 확인
        if (scriptableObject.GetClip(clipNumber) == null)
        {
            Debug.Log($"{scriptableObject.name}의 {clipNumber}번 항목이 존재하지 않음");
            return false;
        }
        else if (audioSource.clip != scriptableObject.GetClip(clipNumber))
        {
            Debug.Log($"필요한 사운드 클립: {scriptableObject.GetClip(clipNumber)}, 현재 할당된 사운드 클립: {audioSource.clip.name}");
        }
        return true;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
