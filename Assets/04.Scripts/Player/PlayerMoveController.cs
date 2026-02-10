/*
이 스크립트는 플레이어 이동을 관리함
플레이어 이동과 관련이 없는 스크립트는 이 코드에 작성하면 안된다.
*/
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements.Experimental;

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
    // 프로퍼티
    public Vector2 MoveVector
    {
        get {return _moveVector;}
        set
        {
            // 사망 상태라면 외부에서 어떤 값을 넣으려고 해도 0으로 고정
            if (_playerStatController.Dead == true) _moveVector = Vector2.zero;
            else _moveVector = value;
        }
    }
    
    public Vector2 InputVector
    {
        get {return _inputVector;}
        set
        {
            // 사망 상태라면 외부에서 어떤 값을 넣으려고 해도 0으로 고정
            if (_playerStatController.Dead == true) _inputVector = Vector2.zero;
            else _inputVector = value;
        }
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
        _currentPos = _playerManager.gameObject.transform.position;
        _lastPos = _playerManager.gameObject.transform.position;
    }
#endregion
    
#region 이벤트 변수들
    private void AddToEvent()
    {
        _playerEventController.Death += OnEventDeath;
    }
    
    private void OnEventDeath()
    {
        MoveVector = Vector2.zero;
        InputVector = Vector2.zero;
    }
#endregion

#region Input System Package 메서드
    // 플레이어 이동 처리하는 메서드
    public void OnMove(InputAction.CallbackContext context)
    {
        InputVector = context.ReadValue<Vector2>();
        if (_playerStatController.Dead) return;
        
        // 프로퍼티에서 set 로직에 선언한 대로, 죽은 상태라면 알아서 Vector2.zero를 대입한다.
        MoveVector = new Vector2(InputVector.x, InputVector.y);

        // 플레이어가 움직이기 시작했는지, 멈추기 시작했는지 알리기
        if (MoveVector == Vector2.zero) _playerEventController.CallStop();
        else _playerEventController.CallMove();
    }
#endregion
}