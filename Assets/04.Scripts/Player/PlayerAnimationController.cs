/*
플레이어의 애니메이션을 관리하는 스크립트.
플레이어의 애니메이션 작동은 이곳에서 관리한다.
*/
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAnimationController : MonoBehaviour
{
    #region 애니메이터 패러미터 ID
    private static readonly int IsHurt = Animator.StringToHash("isHurt");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    #endregion

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
        if(_playerEventController != null)
        {
            RemoveFromEvent();
            AddToEvent();
        }
    }

    private void OnDisable()
    {
        RemoveFromEvent();
    }

    #endregion

    [SerializeField] private PlayableDirector _director;
    

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
    private void OnEventHurt()
    {
        _animator.SetTrigger(IsHurt);
    }
    
    private void OnEventDeath()
    {
        _animator.SetBool(IsDead, true);
    }
    
    private void OnEventRevive()
    {
        _animator.SetBool(IsDead, false);
    }
    
    private void OnEventMove()
    {
        _animator.SetBool(IsMoving, true);
        if (_playerMoveController.InputVector.x < 0) _spriteRenderer.flipX = true;
        else if (_playerMoveController.InputVector.x > 0) _spriteRenderer.flipX = false;
    }
    
    private void OnEventStop()
    {
        _animator.SetBool(IsMoving, false);
    }
#endregion
}
