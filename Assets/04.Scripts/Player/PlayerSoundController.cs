using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
#region 변수들
    private SoundManager _soundManager;
    private PlayerManager _playerManager;
    private PlayerEventController _playerEventController;
#endregion
    
#region SetUp 함수
    public void SetUp()
    {
        _soundManager = SoundManager.Instance;
        _playerManager = PlayerManager.Instance;
        _playerEventController = _playerManager.PlayerEventController;
    }
    
    public void AddOnEvent()
    {
        _playerEventController.Hurt += OnEventHurt;
    }
    
    private void OnEventHurt()
    {
        
    }
#endregion
}
