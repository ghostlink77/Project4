/*
플레이어의 모든 상태를 관리하는 스크립트

플레이어와 관련된 모든 Update, FixedUpdate 항목은 여기에서 작성하도록 한다.
*/
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerMoveController _playerMoveController;
    PlayerStatController _playerStatController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetPlayerComponent();
    }
    
    // 플레이어 컴포넌트 초기화하는 메서드
    void GetPlayerComponent()
    {
        _playerMoveController = GetComponent<PlayerMoveController>();
        _playerStatController = GetComponent<PlayerStatController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어 이동 애니메이션 설정
        _playerMoveController.SetMoveAnimation();
    }
    
    // 플레이어에게 데미지 입히는 메서드
    public void GetHurt(int dmg) => _playerStatController.GetHurt(dmg);
}
