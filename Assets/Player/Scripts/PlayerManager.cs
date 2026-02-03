/*
플레이어의 모든 상태를 관리하는 스크립트

플레이어와 관련된 모든 Update, FixedUpdate 항목은 여기에서 작성하도록 한다.
*/
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerMoveController _playerMoveController;
    private PlayerStatController _playerStatController;
    PlayerItemController _playerItemController;
    private SoundManager _soundManager;
    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetPlayerComponent();
    }
    
    void Start()
    {
        _playerStatController.SetUp(_animator, _soundManager);
        _playerMoveController.SetUp(_animator, _playerStatController.MoveSpeed);
        _playerItemController.SetUp(
            _playerStatController.WeaponSlotSize, 
            _playerStatController.PassiveItemSlotSize,
            _playerStatController.TurretSlotSize
        );
    }

    // 플레이어 컴포넌트 초기화하는 메서드
    void GetPlayerComponent()
    {
        _playerMoveController = GetComponent<PlayerMoveController>();
        _playerStatController = GetComponent<PlayerStatController>();
        _playerItemController = GetComponent<PlayerItemController>();

        _soundManager = SoundManager.Instance;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 플레이어 이동 애니메이션 설정
        _playerMoveController.SetMoveAnimation();
    }

    // 플레이어에게 데미지 입히는 메서드
    public void GetHurt(int dmg) => _playerStatController.TakeDamage(dmg);
}
