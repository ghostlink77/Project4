using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public enum SoundType {Player, Enemy}

public class SoundManager : MonoBehaviour
{
    // 싱글톤을 위한 스태틱 변수
    public static SoundManager Instance {get; private set;}
    
    [Header("Audio Sources")]
    [Tooltip("플레이어 관련 사운드 출력")]
    [SerializeField]
    private AudioSource playerSource;
    [Tooltip("적 관련 사운드 출력")]
    [SerializeField]
    private AudioSource enemySource;

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
    
    public void PlaySFX(SoundType type, AudioClip clip)
    {
        switch(type)
        {
            case SoundType.Player:
                playerSource.PlayOneShot(clip);
                break;
            case SoundType.Enemy:
                enemySource.PlayOneShot(clip);
                break;
        }
    }
}
