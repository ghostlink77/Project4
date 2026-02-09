/*
플레이어 캐릭터의 게임 플레이 도중의 상태 변화, 성장 등을 관리하는 스크립트
*/
using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatController : MonoBehaviour, IDamageable
{
    [Header("스크립터블 오브젝트들")]
    [SerializeField]
    private PlayerDefaultData playerDefaultData;

    [SerializeField]
    private SoundData playerSound;
    
    // 제미나이의 조언으로 추가.
    // 사망 후 부활 전에 사망 애니메이션이 출력되는 시간을 마련하기 위해 캐싱해두는 변수
    private WaitForSeconds _reviveDelayWaitAction;
    
    // 플레이어 스탯
    
    private int _currentLevel;
    public int CurrentLevel {get => _currentLevel; set => _currentLevel = value;}
    public int CurrentExp { get; private set; }
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
    
    // 플레이어 부활까지 걸리는 시간이기 때문에 조정할 필요가 있음. 따라서 SerializeField를 적용함.
    [SerializeField]
    private float _reviveDelayTime = 1.5f;
    public float ReviveDelayTime
    {
        get => _reviveDelayTime;
        set => _reviveDelayTime = (value <= 0) ? 0 : value;
    }
    private int _weaponSlotSize;
    public int WeaponSlotSize {get => _weaponSlotSize; set => _weaponSlotSize = value;}
    private int _passiveItemSlotSize;
    public int PassiveItemSlotSize {get => _passiveItemSlotSize; set => _passiveItemSlotSize = value;}
    private int _turretSlotSize;
    public int TurretSlotSize {get => _turretSlotSize; set => _turretSlotSize = value;}

    private Animator _animator;
    private SoundManager _sm;
    // 필요한 데이터를 PlayerManager에서 받아오는 메서드
    // 매개변수로 받아오므로, 필요할 때마다 매개변수에 추가해야 함.
    public void SetUp()
    {
        resetPlayerStat();
        _animator = PlayerManager.Instance.Animator;
        _sm = SoundManager.Instance;
        
        // 부활까지 걸리는 시간 캐싱하여 재사용
        _reviveDelayWaitAction = new WaitForSeconds(ReviveDelayTime);
    }

    //플레이어 데이터를 스크립터블 오브젝트에 있는 걸로 초기화하는 메서드
    private void resetPlayerStat()
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
    public void TakeDamage(int damage)
    {
        // 플레이어가 이미 죽은 경우 메서드 미적용
        if (Dead == true) return;
        
        // 애니메이션 재생을 위한 트리거
        _animator.SetTrigger("isHurt");
        
        // 방어력 적용해서 데미지 적용
        int calcDmg = CalculateReducedDmg(damage, Defense);
        CurrentHp -= calcDmg;
        Debug.Log($"{calcDmg} 적용, 남은 체력: {CurrentHp}");

        if (CurrentHp <= 0) CurrentHp = 0;

        // 플레이어 죽었는지 확인
        CheckDead();
        // 플레이어 피격 이펙트 출력
        ActivateHurtAnimation();
    }
    int CalculateReducedDmg(int damage, int defense)
    {
        float value = damage * 100 / (100 + defense);
        return (int)Math.Round(value);
    }
    
    // 플레이어 다치는 애니메이션 실행
    void ActivateHurtAnimation()
    {
        if (_animator == null)
        {
            Debug.LogError("애니메이터 비어있음");
            return;
        }

        if (_sm != null && playerSound != null) _sm.PlayPlayerSFX(playerSound.GetClip(0));
        else if (_sm == null) Debug.LogError("사운드매니저 null");
        else if (playerSound == null) Debug.LogError("플레이어 사운드 null");
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
        if (Dead == true) PlayerDeadSequence();
    }
    
    // 플레이어가 죽으면 실행할 효과들
    public void PlayerDeadSequence()
    {
        // 플레이어 애니메이터 사망 트리거 실행
        _animator.SetBool("isDead", Dead);
        PlayerManager.Instance.PlayerMoveController.MoveVector = Vector2.zero;
        StartCoroutine(AfterReviveDelay());
    }
    
    private IEnumerator AfterReviveDelay()
    {
        Animator animator = PlayerManager.Instance.Animator;
        
        yield return _reviveDelayWaitAction;
        PlayerReviveStatusSetting();
    }

    // 플레이어 부활시킬 때 스탯 변화를 적용할 메서드
    private void PlayerReviveStatusSetting()
    {
        if (CheckRevivable() == true)
        {
            // 목숨 차감
            Life -= 1;
            Dead = false;
            // 사망 애니메이션 출력을 위한 설정
            PlayerManager.Instance.Animator.SetBool("isDead", false);
            // 체력 최대 체력으로 초기화
            CurrentHp = MaxHp;
            Debug.Log($"부활 후 남은 목숨: {Life}, 플레이어 부활함");
        }
        else
        {
            Debug.Log("플레이어 사망");
            // 게임 오버 알리는 이벤트 추가
        }
    }
    
    // 부활 가능한지 판단하는 메서드
    private bool CheckRevivable()
    {
        if (Life > 0)
        {
            Debug.Log($"목숨 {Life}개, 부활 가능");
            return true;
        }
        else
        {
            Debug.Log($"목숨 {Life}개, 부활 불가능");
            return false;
        }
    }
}
