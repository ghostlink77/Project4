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
        _weaponEventController = GetComponent<WeaponEventController>();
        _audioSource = GetComponent<AudioSource>();
        AddToEvent();
    }
    
    #region 이벤트 관련 변수
    // 이벤트 구독
    private void AddToEvent()
    {
        _weaponEventController.OnShoot += OnEventOnShoot;
    }
    
    // 이벤트 구독 해제
    private void RemoveFromEvent()
    {
        _weaponEventController.OnShoot -= OnEventOnShoot;
    }
    
    // 발사 시 사운드 출력
    private void OnEventOnShoot()
    {
        if (ObjectIsNull(_audioSource, "AudioSource")) return;
        if (ObjectIsNull(_weaponShootSound, "Weapon Shoot Sound")) return;
        _audioSource.PlayOneShot(_weaponShootSound);
    }
    #endregion
    
    #region 점검 스크립트
    private bool ObjectIsNull(Object obj, string name)
    {
        if (obj == null)
        {
            Debug.LogWarning($"{name}이(가) null임");
            return true;
        }
        return false;
    }
    #endregion
}
