using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
#region 스크립트 참조 변수들
    private SoundManager _soundManager;
    private PlayerManager _playerManager;
    private PlayerEventController _playerEventController;
#endregion

#region 사운드 클립들
    [Header("플레이어 피해 입는 사운드 클립")]
    [SerializeField]
    private AudioClip _hurtClip;
    #endregion

    #region 이벤트 생명주기 함수
    private void OnEnable()
    {
        if (_playerEventController != null) AddOnEvent();
    }

    private void OnDisable()
    {
        RemoveFromEvent();
    }
    #endregion

    #region SetUp 함수
    public void SetUp()
    {
        _soundManager = SoundManager.Instance;
        _playerManager = PlayerManager.Instance;
        _playerEventController = _playerManager.PlayerEventController;
        AddOnEvent();
    }
    
    #endregion
    #region 이벤트 관련 메서드
    private void AddOnEvent()
    {
        _playerEventController.Hurt += OnEventHurt;
    }
    
    private void RemoveFromEvent()
    {
        _playerEventController.Hurt -= OnEventHurt;
    }
    
    private void OnEventHurt()
    {
        if (IsNull(_hurtClip)) return;
        _soundManager.PlaySFX(SoundType.Player, _hurtClip);
    }
    #endregion
    
    #region null 점검 메서드
    private bool IsNull<T>(T obj) where T : UnityEngine.Object
    {
        if (obj == null)
        {
            Debug.Log($"{nameof(obj)} 오브젝트 null임");
            return true;
        }
        else return false;
    }
    #endregion
}
