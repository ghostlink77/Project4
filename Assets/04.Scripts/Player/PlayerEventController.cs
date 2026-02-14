/*
플레이어의 이벤트를 관리하는 스크립트
이벤트를 사용해야 할 경우, 이 스크립트를 호출하여 사용하도록 한다.
*/
using System;
using UnityEngine;

public class PlayerEventController : MonoBehaviour
{
#region PlayerStatusEvents
    // 플레이어 상태 관련 이벤트
    public event Action Death, Revive, Hurt;
    
    public void CallDeath() => Death?.Invoke();
    public void CallRevive() => Revive?.Invoke();
    public void CallHurt() => Hurt?.Invoke();
#endregion

#region PlayerMoveEvents
    // 플레이어 이동 관련 이벤트
    public event Action Move, Stop;

    public void CallMove() => Move?.Invoke();
    public void CallStop() => Stop?.Invoke();
#endregion

#region PlayerItemEvents
    // 플레이어 아이템 수집 관련 이벤트
    public event Action ScrapCollected;

    public void CallScrapCollected() => ScrapCollected?.Invoke();
#endregion
}
