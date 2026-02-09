/*
플레이어의 애니메이션을 관리하는 스크립트.
플레이어의 애니메이션 작동은 이곳에서 관리한다.
*/
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private PlayerEventController _playerEventController;

    public void SetUp()
    {
        _animator = PlayerManager.Instance.Animator;
        _playerEventController = PlayerManager.Instance.PlayerEventController;
        AddToEvent();
    }
    
    // 등록할 이벤트의 목록
    private void AddToEvent()
    {
        _playerEventController.Hurt += OnEventHurt;
    }
    
    // Hurt 이벤트에 추가할 메서드
    private void OnEventHurt()
    {
        Debug.Log("플레이어 피격 애니메이션 재생");
        _animator.SetTrigger("isHurt");
    }
    
    // Death 이벤트에 추가할 메서드
    private void OnEventDeath()
    {
        Debug.Log("플레이어 사망 애니메이션 재생");
        _animator.SetBool("isDead", true);
    }
}
