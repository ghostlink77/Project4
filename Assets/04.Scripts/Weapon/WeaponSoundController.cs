using UnityEngine;

public class WeaponSoundController : MonoBehaviour
{
    #region 필요한 변수
    [SerializeField]
    private string _weaponShootClipName;
    private float _ShootSoundLength;
    #endregion

    #region 스크립트 참조 변수
    private WeaponEventController _weaponEventController;
    private AudioManager _audioManager;
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
        AddToEvent();
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        if (_audioManager == null) Debug.LogError("오디오 매니저가 없음");
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
        if (_audioManager != null && _weaponShootClipName != null) _audioManager.PlayAtPoint(_weaponShootClipName, transform.position);//_audioManager.Play(AudioType.BulletSFX, _weaponShootClipName);
        Debug.Log("무기 발사음 출력됨");
    }
    #endregion
}
