/*
플레이어 캐릭터의 게임 플레이 도중의 상태 변화, 성장 등을 관리하는 스크립트
*/
using System;
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
    
    // 플레이어 스탯
    
    public int CurrentLevel {get; set;}
    public int CurrentExp { get; private set; }
    public int MaxHp {get; set;}
    public int CurrentHp {get; set;}
    public int Defense  {get; set;}
    public int HpGenSpeed {get; set;}
    // 플레이어 스탯 반환 및 설정하는 함수
    public float MoveSpeed {get; set;}
    public float ItemGetRadius {get; set;}
    public float Luck {get; set;}
    public float Growth {get; set;}
    public float Greed {get; set;}
    public float Curse {get; set;}
    public int Life {get; set;}
    public bool Dead {get; set;}
    public int WeaponSlotSize{get; set;}
    public int PassiveItemSlotSize{get; set;}
    public int TurretSlotSize {get; set;}
    
    private PlayerEventController _playerEventController;

    // 필요한 데이터를 PlayerManager에서 받아오는 메서드
    // 매개변수로 받아오므로, 필요할 때마다 매개변수에 추가해야 함.
    public void SetUp()
    {
        resetPlayerStat();
        _playerEventController = PlayerManager.Instance.PlayerEventController;
    }
    
    // 이벤트에 추가하는 함수들
    private void AddToEvent()
    {
        CheckDead();
    }
    
    // Hurt 이벤트 활성화 시 작동할 메서드
    private void AddToHurt()
    {
        
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

        // 플레이어 피격 이벤트 실행
        _playerEventController.CallHurt();
        
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
    
    //플레이어 죽음 상태 설정하는 메서드
    private void CheckDead() => Dead = CurrentHp <= 0;
}
