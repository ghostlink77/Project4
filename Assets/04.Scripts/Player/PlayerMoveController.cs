/*
이 스크립트는 플레이어 이동을 관리함
플레이어 이동과 관련이 없는 스크립트는 이 코드에 작성하면 안된다.
*/
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
#region variables
    // 플레이어 스프라이트 뒤집힘 여부 판단하기 위해서 가져옴
    private SpriteRenderer _spriteRenderer;

    // 플레이어 이동 속도를 저장할 변수
    private float _moveSpeed;
    // 유저 입력을 저장할 2차원 벡터 변수
    private Vector2 _inputVector;
    private Vector2 _moveVector;
    // 플레이어 위치 기록용 2차원 벡터 변수들
    // 마지막 위치
    private Vector2 _lastPos;
    // 현재 위치
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
            // 사망 상태라면 외부에서 어떤 값을 넣으려고 해도 0으로 고정
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
    
    // 스크립트 변수들에 할당하는 코드 모아둔 메서드
    private void ScriptVariableSetup()
    {
        _playerManager = PlayerManager.Instance;
        _playerStatController = _playerManager.PlayerStatController;
        _playerEventController = _playerManager.PlayerEventController;
    }
    
    // 일반 변수들에 할당하는 코드 모아둔 메서드
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
    // 플레이어 이동 처리하는 메서드
    public void OnMove(InputAction.CallbackContext context)
    {
        if (_playerStatController.Dead) return;
        InputVector = context.ReadValue<Vector2>();
    }
#endregion

#region 메서드들
    // 매 프레임마다 플레이어를 이동시키는 메서드(PlayerManager의 Update()에 선언해야 함)
    public void MovePlayer()
    {
        
        _currentPos += InputVector * _moveSpeed * Time.deltaTime;

        bool isMoving = CheckMove();
        if (isMoving == false) return;
        transform.position = _currentPos;
        UpdatePosition();
    }
    
    // 이전 위치와 현재 위치가 다르다면 true, 아니라면 false를 반환하는 메서드
    private bool CheckMove()
    {
        if (_lastPos != _currentPos)
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
    
    // 위치 업데이트하는 메서드
    private void UpdatePosition()
    {
        _lastPos = _currentPos;
    }

    // 플레이어 이동 관련 이벤트 판단해서 실행시키는 메서드
    public void InvokeMoveEvents(bool isMoving)
    {
        if (isMoving == true) _playerEventController.CallMove();
        else _playerEventController.CallStop();
    }
#endregion
}