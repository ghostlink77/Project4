/*
플레이어의 모든 상태를 관리하는 스크립트

플레이어와 관련된 모든 Update, FixedUpdate 항목은 여기에서 작성하도록 한다.
*/
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 플레이어 매니저 싱글톤 전용
    public static PlayerManager Instance { get; private set; }

    // 스크립트 컴포넌트들
    public PlayerMoveController _playerMoveController { get; private set; }
    public PlayerStatController _playerStatController { get; private set; }
    public PlayerItemController _playerItemController { get; private set; }
    public SoundManager _soundManager { get; private set; } 
    public Animator _animator { get; private set; }

    void Awake()
    {
        if (SingleTonGenerate() == true)
        {
            _playerMoveController = GetComponent<PlayerMoveController>();
            _playerStatController = GetComponent<PlayerStatController>();
            _playerItemController = GetComponent<PlayerItemController>();
            _animator = GetComponent<Animator>();
        }
    }

    void Start()
    {
        if (Instance == this)
        {
            _soundManager = SoundManager.Instance;
            PlayerComponentSetup();
        }
    }

    bool SingleTonGenerate()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return false;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        return true;
    }

    void PlayerComponentSetup()
    {
        _playerStatController.SetUp(Instance, _soundManager);
        _playerMoveController.SetUp(Instance);
        _playerItemController.SetUp(Instance);
    }

    void Update()
    {
        // 플레이어 이동 애니메이션 설정
        _playerMoveController.SetMoveAnimation();
    }

    // 플레이어에게 데미지 입히는 메서드
    public void GetHurt(int dmg) => _playerStatController.GetHurt(dmg);
}
