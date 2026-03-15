using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
#region variables
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;

    private float _moveSpeed;
    private Vector2 _inputVector;
    private Vector2 _moveVector;

    private Vector2 _lastPos;
    private Vector2 _currentPos;
    
    private PlayerManager _playerManager;
    private PlayerStatController _playerStatController;
    private PlayerEventController _playerEventController;
#endregion
    
#region properties
    public Vector2 InputVector
    {
        get {return _inputVector;}
        set
        {
            if (_playerStatController.Dead == true) _inputVector = Vector2.zero;
            else _inputVector = value;
            _inputVector.Normalize();
        }
    }
    #endregion

    #region 유니티 생명주기 메서드
    private void OnEnable()
    {
        if (_playerEventController != null) AddToEvent();
    }

    private void OnDisable()
    {
        RemoveFromEvent();
    }
    #endregion

    #region 메인 변수들
    public void SetUp()
    {
        ScriptVariableSetup();
        normalVariableSetup();
        AddToEvent();
    }
    
    private void ScriptVariableSetup()
    {
        _playerManager = PlayerManager.Instance;
        _playerStatController = _playerManager.PlayerStatController;
        _playerEventController = _playerManager.PlayerEventController;

        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.freezeRotation = true;
    }
    
    private void normalVariableSetup()
    {
        _moveSpeed = _playerStatController.MoveSpeed;
        Debug.Log($"플레이어 속도: {_moveSpeed}");
        _currentPos = _playerManager.gameObject.transform.position;
        _lastPos = _playerManager.gameObject.transform.position;
    }
#endregion
    
#region 이벤트 변수들
    private void AddToEvent()
    {
        _playerEventController.Death += OnEventDeath;
    }
    
    private void RemoveFromEvent()
    {
        _playerEventController.Death -= OnEventDeath;
    }
    
    private void OnEventDeath()
    {
        InputVector = Vector2.zero;
    }
#endregion

#region Input System Package 메서드
    public void OnMove(InputAction.CallbackContext context)
    {
        if (_playerStatController.Dead) return;
        InputVector = context.ReadValue<Vector2>();
    }
#endregion

#region 메서드들
    public void MovePlayer()
    {
        _currentPos = _rigidBody.position;
        _currentPos += InputVector * _playerStatController.MoveSpeed * Time.fixedDeltaTime;

        _rigidBody.linearVelocity = Vector2.zero;

        bool isMoving = CheckMove();
        if (isMoving == false) return;

        _rigidBody.MovePosition(_currentPos);
        UpdatePosition();
    }
    
    private bool CheckMove()
    {
        if (InputVector != Vector2.zero)
        {
            _playerEventController.CallMove();
            return true;
        }
        else
        {
            _playerEventController.CallStop();
            return false;
        }
    }
    
    private void UpdatePosition()
    {
        _lastPos = _currentPos;
    }

    public void InvokeMoveEvents(bool isMoving)
    {
        if (isMoving == true) _playerEventController.CallMove();
        else _playerEventController.CallStop();
    }
#endregion
}