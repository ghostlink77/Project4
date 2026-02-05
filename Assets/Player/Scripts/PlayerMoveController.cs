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
    // 플레이어 스프라이트 뒤집힘 여부 판단하기 위해서 가져옴
    private SpriteRenderer _spriteRenderer;

    // 플레이어 애니메이션 패러미터를 수정하기 위한 애니메이터
    private Animator _animator;
    // 플레이어 이동 속도를 저장할 변수
    private float _moveSpeed;
    // 유저 입력을 저장할 2차원 벡터 변수
    private Vector2 _inputVector;
    private Vector3 _moveVector;
    
    // 플레이어 위치 기록용 2차원 벡터 변수들
    // 마지막 위치
    private Vector2 _lastPos;
    // 현재 위치
    private Vector2 _currentPos;
    
    public void SetUp(PlayerManager manager)
    {
        _animator = manager.Animator;
        _moveSpeed = manager.PlayerStatController.MoveSpeed;
        Debug.Log($"플레이어 속도 {_moveSpeed}");
        _currentPos = manager.gameObject.transform.position;
        _lastPos = manager.gameObject.transform.position;
        _spriteRenderer = manager.gameObject.GetComponent<SpriteRenderer>();
    }

    // 플레이어 위치를 가져와서 애니메이션 처리를 하도록 하는 메서드
    public void SetMoveAnimation()
    {
        _currentPos = gameObject.transform.position;
        transform.Translate(_moveVector.normalized * _moveSpeed * Time.deltaTime);
        DefinePlayerAnimation(_currentPos, _lastPos);
        _lastPos = _currentPos;
    }
    
    // 플레이어 이동 처리하는 메서드
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
        _moveVector = new Vector3(_inputVector.x, _inputVector.y, 0);
    }
    
    // 플레이어 입력에 따라 애니메이션 스프라이트를 뒤집는 메서드
    private void FlipCharacter(Vector2 input)
    {
        if (input.x < 0) _spriteRenderer.flipX = true;
        else if (input.x > 0) _spriteRenderer.flipX = false;
    }
    
    // 플레이어 이동 관리하는 메서드
    void DefinePlayerAnimation(Vector2 lastPosition, Vector2 nowPosition)
    {
        _animator.SetBool("isMoving", (lastPosition != nowPosition));
        FlipCharacter(_inputVector);
    }
}

