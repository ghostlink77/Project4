/*
총알의 사운드를 관리하는 스크립트
*/
using Unity.VisualScripting;
using UnityEngine;

public class BulletSoundController : MonoBehaviour
{
    [Header("플레이어 사운드 컴포넌트")]
    [SerializeField]
    private AudioSource _audioSource;

    #region 변수들
    [Header("적 맞췄을 때 나는 소리")]
    [SerializeField]
    private AudioClip _hitSound;
    public AudioClip HitSound {get => _hitSound; private set=> _hitSound = value;}
    #endregion
    
    #region 스크립트 참조
    private BulletController _bulletController;
    #endregion
    
    #region 이벤트 메서드
    private void AddEvent()
    {
        _bulletController.Hit += OnEventHit;
    }
    
    private void RemoveEvent()
    {
        _bulletController.Hit -= OnEventHit;
    }

    private void OnEventHit()
    {
        // 일단 임시로 사용. 지금은 방향에 맞춰 소리가 들리지 않는 모노 사운드로만 구현됨.
        SoundManager.Instance.PlaySFX(SoundType.Enemy, _hitSound);
    }
    #endregion

    #region 유니티 생명주기 메서드
    void Awake()
    {
        _bulletController = GetComponent<BulletController>();
    }

    private void OnEnable()
    {
        if (_bulletController != null) AddEvent();
    }

    private void OnDisable()
    {
        RemoveEvent();
    }
    #endregion
    
    private void PlaySound(AudioClip clip)
    {
        if (IsObjectNull(_audioSource)) return;
        if (IsObjectNull(clip))return;
        _audioSource.PlayOneShot(clip);
    }

    #region null 점검 스크립트
    private bool IsObjectNull<T>(T obj)
    {
        if (obj == null)
        {
            Debug.Log($"{nameof(obj)} 오브젝트가 null임");
            return true;
        }
        else return false;
    }
    #endregion
}
