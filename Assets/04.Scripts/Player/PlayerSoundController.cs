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
    
#region SetUp 함수
    public void SetUp()
    {
        _soundManager = SoundManager.Instance;
        _playerManager = PlayerManager.Instance;
        _playerEventController = _playerManager.PlayerEventController;
        AddOnEvent();
    }
    
    public void AddOnEvent()
    {
        _playerEventController.Hurt += OnEventHurt;
    }
    
    private void OnEventHurt()
    {
        if (_hurtClip != null) _soundManager.PlayPlayerSFX(_hurtClip);
        else Debug.LogError($"{_hurtClip.name}가 null임");
    }
#endregion
}
