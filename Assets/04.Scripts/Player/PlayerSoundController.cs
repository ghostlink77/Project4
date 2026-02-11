using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
#region 변수들
    private SoundManager _soundManager;
#endregion
    
#region SetUp 함수
    public void SetUp()
    {
        _soundManager = SoundManager.Instance;
    }
#endregion
}
