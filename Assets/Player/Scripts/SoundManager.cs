using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 싱글톤을 위한 스태틱 변수
    public static SoundManager Instance {get; private set;}
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource playerSource;
    
    private void Awake()
    {
        // 인스턴스가 비어있다면 자신을 할당
        if (Instance == null)
        {
            Instance = this;
            
            //씬이 바뀌어도 파괴되지 않게 설정
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재한다면 새로 생성된 오브젝트 파괴
            Destroy(gameObject);
        }
    }
    
    // playerSource에서 사운드 클립 재생
    public void PlayPlayerSFX(AudioClip clip)
    {
        playerSource.PlayOneShot(clip);
    }
}
