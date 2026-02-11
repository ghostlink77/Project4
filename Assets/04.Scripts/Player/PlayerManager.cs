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
    public PlayerMoveController PlayerMoveController { get; private set; }
    public PlayerStatController PlayerStatController { get; private set; }
    public PlayerItemController PlayerItemController { get; private set; }
    public PlayerAnimationController PlayerAnimationController { get; private set; }
    public PlayerEventController PlayerEventController { get; private set; }
    public PlayerSoundController PlayerSoundController { get; private set; }
    public SoundManager SoundManager { get; private set; } 
    public Animator Animator { get; private set; }

    void Awake()
    {
        if (SingleTonGenerate() == true)
        {
            PlayerMoveController = GetComponent<PlayerMoveController>();
            PlayerStatController = GetComponent<PlayerStatController>();
            PlayerItemController = GetComponent<PlayerItemController>();
            PlayerAnimationController = GetComponent<PlayerAnimationController>();
            PlayerEventController = GetComponent<PlayerEventController>();
            Animator = GetComponent<Animator>();
        }
    }

    void Start()
    {
        if (Instance == this)
        {
            SoundManager = SoundManager.Instance;
            PlayerComponentSetup();
        }
    }

    void Update()
    {
        PlayerMoveController.MovePlayer();
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
        PlayerStatController.SetUp();
        PlayerMoveController.SetUp();
        PlayerItemController.SetUp();
        PlayerAnimationController.SetUp();
    }

    // 플레이어에게 데미지 입히는 메서드
    public void GetHurt(int dmg) => PlayerStatController.TakeDamage(dmg);
}
