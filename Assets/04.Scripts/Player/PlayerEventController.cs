/*
플레이어의 이벤트를 관리하는 스크립트
이벤트를 사용해야 할 경우, 이 스크립트를 호출하여 사용하도록 한다.
*/
using System;
using UnityEngine;

public class PlayerEventController : MonoBehaviour
{
#region PlayerStatusEvents
    public event Action Death, Revive, Hurt;
    
    public void CallDeath() => Death?.Invoke();
    public void CallRevive() => Revive?.Invoke();
    public void CallHurt() => Hurt?.Invoke();
#endregion

#region PlayerMoveEvents
    public event Action Move, Stop;

    public void CallMove() => Move?.Invoke();
    public void CallStop() => Stop?.Invoke();
#endregion

#region PlayerItemEvents
    public event Action ScrapCollected;

    public void CallScrapCollected() => ScrapCollected?.Invoke();
#endregion
}
