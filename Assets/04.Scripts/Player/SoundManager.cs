using System.Collections;
using UnityEngine;

public enum SoundType {Player, Enemy}

public class SoundManager : MonoBehaviour
{
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
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
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
