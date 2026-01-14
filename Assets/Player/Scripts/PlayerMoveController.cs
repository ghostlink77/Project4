/*
이 스크립트는 플레이어 이동을 관리함
플레이어 이동과 관련이 없는 스크립트는 이 코드에 작성하면 안된다.
*/
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements.Experimental;

public class PlayerMoveController : MonoBehaviour
{
    // 플레이어 이동에 사용되는 스탯을 가져오기 위함
    private PlayerStatController _playerStat;
    private SpriteRenderer _spriteRenderer;

    // 플레이어 애니메이션 패러미터를 수정하기 위한 애니메이터
    private Animator _animator;
    // 플레이어 이동 속도를 저장할 변수
    private float _moveSpeed;
    // 유저 입력을 저장할 2차원 벡터 변수
    private Vector2 _inputVector;
    private Vector3 _moveVector;
    
    // 플레이어 위치 기록용 2차원 벡터 변수들
    private Vector2 _lastPos;
    private Vector2 _currentPos;
    
    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _playerStat = gameObject.GetComponent<PlayerStatController>();
    }
    
    void Start()
    {
        _moveSpeed = _playerStat.MoveSpeed;
        _currentPos = gameObject.transform.position;
        _lastPos = gameObject.transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 플레이어 위치 가져오기
        _currentPos = gameObject.transform.position;
        
        // 이동 처리
        FlipCharacter(_inputVector);
        transform.Translate(_moveVector.normalized * _moveSpeed * Time.deltaTime);

        // 애니메이션 처리
        DefinePlayerAnimation(_currentPos, _lastPos);
        
        // 플레이어 위치 기록하기
        _lastPos = _currentPos;
    }
    
    // 플레이어 이동 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
        _moveVector = new Vector3(_inputVector.x, _inputVector.y, 0);
    }
    
    private void FlipCharacter(Vector2 input)
    {
        if (input.x < 0) {_spriteRenderer.flipX = true;}
        else if (input.x > 0) {_spriteRenderer.flipX = false;}
    }
    
    // 플레이어 이동 관리하는 프로그램
    void DefinePlayerAnimation(Vector2 value1, Vector2 value2)
    {
        _animator.SetBool("isMoving", (value1 != value2));
    }
}

