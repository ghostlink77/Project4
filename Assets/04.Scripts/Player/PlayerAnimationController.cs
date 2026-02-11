/*
플레이어의 애니메이션을 관리하는 스크립트.
플레이어의 애니메이션 작동은 이곳에서 관리한다.
*/
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
#region variables
    private Animator _animator;
    private PlayerEventController _playerEventController;
    private PlayerManager _playerManager;
    private PlayerMoveController _playerMoveController;
    private SpriteRenderer _spriteRenderer;
#endregion
#region 유니티 생명주기 변수들
    private void OnEnable()
    {
        if(_playerEventController != null) AddToEvent();
    }

    private void OnDisable()
    {
        RemoveFromEvent();
    }

#endregion

    public void SetUp()
    {
        _playerManager = PlayerManager.Instance;
        _playerEventController = _playerManager.PlayerEventController;
        _playerMoveController = _playerManager.PlayerMoveController;
        _animator = _playerManager.Animator;
        _spriteRenderer = _playerManager.SpriteRenderer;
        AddToEvent();
    }

#region 이벤트 메서드
    // 등록할 이벤트의 목록
    private void AddToEvent()
    {
        _playerEventController.Hurt += OnEventHurt;
        _playerEventController.Death += OnEventDeath;
        _playerEventController.Revive+= OnEventRevive;
        _playerEventController.Move += OnEventMove;
        _playerEventController.Stop += OnEventStop;
    }
    
    private void RemoveFromEvent()
    {
        _playerEventController.Hurt -= OnEventHurt;
        _playerEventController.Death -= OnEventDeath;
        _playerEventController.Revive -= OnEventRevive;
        _playerEventController.Move -= OnEventMove;
        _playerEventController.Stop -= OnEventStop;
    }
    // Hurt 이벤트에 추가할 메서드
    private void OnEventHurt()
    {
        _animator.SetTrigger("isHurt");
    }
    
    // Death 이벤트에 추가할 메서드
    private void OnEventDeath()
    {
        _animator.SetBool("isDead", true);
    }
    
    // Revive 이벤트에 추가할 메서드
    private void OnEventRevive()
    {
        _animator.SetBool("isDead", false);
    }
    
    private void OnEventMove()
    {
        // Debug.Log("플레이어 이동 시작");
        _animator.SetBool("isMoving", true);
        if (_playerMoveController.InputVector.x < 0) _spriteRenderer.flipX = true;
        else if (_playerMoveController.InputVector.x > 0) _spriteRenderer.flipX = false;
    }
    
    private void OnEventStop()
    {
        // Debug.Log("플레이어 이동 멈춤");
        _animator.SetBool("isMoving", false);
    }
#endregion
}
