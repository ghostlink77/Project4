using UnityEngine;

public class WeaponSoundController : MonoBehaviour
{
    #region 필요한 변수
    [SerializeField]    
    private AudioClip _weaponShootSound;
    private float _ShootSoundLength;
    #endregion

    #region 스크립트 참조 변수
    private WeaponEventController _weaponEventController;
    private SoundManager _soundManager;
    #endregion

    #region 컴포넌트 참조변수
    private AudioSource _audioSource;
    #endregion

    #region 유니티 생명주기 함수
    private void OnEnable()
    {
        if (_weaponEventController != null) AddToEvent();
    }

    private void OnDisable()
    {
        if(_weaponEventController != null) RemoveFromEvent();
    }
    #endregion

    public void SetUp()
    {
        if(!TryGetComponent<WeaponEventController>(out _weaponEventController))
        Debug.LogError($"{_weaponEventController.GetType()} null임");
        if(!TryGetComponent<AudioSource>(out _audioSource))
        Debug.LogError($"{_audioSource.GetType()} null임");
        AddToEvent();
    }

    private void Start()
    {
        _soundManager = SoundManager.Instance;
        if (_soundManager == null) Debug.LogError("사운드매니저가 씬에 없음");
    }

    #region 이벤트 관련 변수
    private void AddToEvent()
    {
        _weaponEventController.OnShoot += PlayShootSound;
    }
    
    private void RemoveFromEvent()
    {
        _weaponEventController.OnShoot -= PlayShootSound;
    }
    
    private void PlayShootSound()
    {
        _soundManager.PlaySFX(SoundType.Player, _weaponShootSound);
        Debug.Log("무기 발사음 출력됨");
    }
    #endregion
}
