/*
플레이어 캐릭터의 게임 플레이 도중의 상태 변화, 성장 등을 관리하는 스크립트
*/
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    [Header("스크립터블 오브젝트들")]
    [SerializeField]
    private PlayerDefaultData playerDefaultData;

    [SerializeField]
    private SoundData playerSound;
    
    // 플레이어 스탯
    private int _currentLevel;
    public int CurrentLevel {get => _currentLevel; set => _currentLevel = value;}
    private int _maxHp;
    public int MaxHp {get => _maxHp; set => _maxHp = value;}
    private int _currentHp;
    public int CurrentHp {get => _currentHp; set => _currentHp = value;}
    private int _defense;
    public int Defense {get => _defense; set => _defense = value;}
    private int _hpGenSpeed;
    public int HpGenSpeed {get => _hpGenSpeed; set => _hpGenSpeed = value;}
    private float _moveSpeed;
    // 플레이어 스탯 반환 및 설정하는 함수
    public float MoveSpeed {get => _moveSpeed; set => _moveSpeed = value;}
    private float _itemGetRadius;
    public float ItemGetRadius {get => _itemGetRadius; set => _itemGetRadius = value;}
    private float _luck;
    public float Luck {get => _luck; set => _luck = value;}
    private float _growth;
    public float Growth {get => _growth; set => _growth = value;}
    private float _greed;
    public float Greed {get => _greed; set => _greed = value;}
    private float _curse;
    public float Curse {get => _curse; set => _curse = value;}
    private int _life;
    public int Life {get => _life; set => _life = value;}
    private bool _dead = false;
    public bool Dead {get => _dead; set => _dead = value;}
    private int _weaponSlotSize;
    public int WeaponSlotSize {get => _weaponSlotSize; set => _weaponSlotSize = value;}
    private int _passiveItemSlotSize;
    public int PassiveItemSlotSize {get => _passiveItemSlotSize; set => _passiveItemSlotSize = value;}
    private int _turretSlotSize;
    public int TurretSlotSize {get => _turretSlotSize; set => _turretSlotSize = value;}

    private Animator animator;
    private SoundManager soundManager;
    
    void Awake()
    {
        resetPlayerStat();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        soundManager = SoundManager.Instance;
    }
    
    //플레이어 데이터를 스크립터블 오브젝트에 있는 걸로 초기화하는 메서드
    void resetPlayerStat()
    {
        // 플레이어 스테이터스
        CurrentLevel = playerDefaultData.DefaultLevel;
        MaxHp = playerDefaultData.DefaultMaxHP;
        CurrentHp = MaxHp;
        Defense = playerDefaultData.DefaultDef;
        HpGenSpeed = playerDefaultData.DefaultHpGenSpeed;
        MoveSpeed= playerDefaultData.DefaultSpeed;
        ItemGetRadius = playerDefaultData.DefaultReceiveRadius;
        Luck = playerDefaultData.DefaultLuck;
        Growth = playerDefaultData.DefaultGrowth;
        Greed = playerDefaultData.DefaultGreed;
        Curse = playerDefaultData.DefaultCurse;
        Life = playerDefaultData.DefaultLife;
        
        // 슬롯들
        WeaponSlotSize = playerDefaultData.DefaultWeaponSlotSize;
        PassiveItemSlotSize = playerDefaultData.DefaultPassiveItemSlotSize;
        TurretSlotSize = playerDefaultData.DefaultTurretSlotSize;
    }
    
    //플레이어 hp에 데미지 가하는 함수
    // 플레이어의 hp를 치료하는 효과는 다른 함수로 구현하도록 한다.
    public void GetHurt(int damage)
    {
        // 플레이어가 이미 죽은 경우 메서드 미적용
        if (Dead == true) return;
        
        // 플레이어 피격 이펙트 출력
        ActivateHurtAnimation();

        // 방어력 적용해서 데미지 적용
        int calcDmg = CalculateReducedDmg(damage, Defense);
        CurrentHp -= calcDmg;
        Debug.Log($"{calcDmg} 적용, 남은 체력: {CurrentHp}");

        if (CurrentHp <= 0) CurrentHp = 0;
    }
    int CalculateReducedDmg(int damage, int defense)
    {
        float value = damage * 100 / (100 + defense);
        return (int)Math.Round(value);
    }
    
    // 플레이어 다치는 애니메이션 실행
    void ActivateHurtAnimation()
    {
        animator.SetTrigger("isHurt");
        soundManager.PlayPlayerSFX(playerSound.GetClip(0));
    }
    
    // 플레이어의 현재 hp가 0 이하인지 확인하는 메서드
    bool CheckHpZero()
    {
        if (CurrentHp <= 0) return true;
        else return false;
    }
    
    //플레이어 죽음 상태 설정하는 메서드
    public void CheckDead()
    {
        Dead = CheckHpZero();
        animator.SetBool("isDead", Dead);
    }
}
