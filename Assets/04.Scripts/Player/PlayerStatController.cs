/*
플레이어 캐릭터의 게임 플레이 도중의 상태 변화, 성장 등을 관리하는 스크립트
*/
using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatController : MonoBehaviour, IDamageable
{
    #region 스크립터블 오브젝트
    [Header("스크립터블 오브젝트들")]
    [SerializeField]
    private PlayerDefaultData playerDefaultData;
    #endregion
    
    #region 플레이어 스탯 변수들
    public int CurrentLevel {get; set;}
    public int CurrentExp { get; private set; }
    public int MaxHp {get; set;}
    public int CurrentHp {get; set;}
    public int Defense  {get; set;}
    public int HpGenSpeed {get; set;}
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
    #endregion

    #region 플레이어 부활 시간 관련 변수
    [SerializeField]
    private float _reviveDelayTime = 1.5f;
    private float ReviveDelayTime
    {
        get => _reviveDelayTime;
        set => _reviveDelayTime = (value <= 0) ? 0 : value;
    }
    
    private WaitForSeconds _reviveDelayAction;
    #endregion
    
    #region 스크립트 참조변수
    private PlayerEventController _playerEventController;
    #endregion

    # region 유니티 생명주기 함수들
    private void OnEnable()
    {
        if (_playerEventController != null)
        {
            RemoveFromEvent();
            AddToEvent();
        }
    }

    private void OnDisable()
    {
        RemoveFromEvent();
    }

    public void SetUp()
    {
        resetPlayerStat();
        _playerEventController = PlayerManager.Instance.PlayerEventController;
        _reviveDelayAction = new WaitForSeconds(ReviveDelayTime);
    }
    #endregion
    
    #region 이벤트 관련 메서드
    private void AddToEvent()
    {
        _playerEventController.Death += AddToDeath;
        _playerEventController.Revive += AddToRevive;
    }
    
    private void RemoveFromEvent()
    {
        _playerEventController.Death -= AddToDeath;
        _playerEventController.Revive -= AddToRevive;
    }

    private void AddToDeath()
    {
        Debug.Log("플레이어 사망");
        PlayerManager.Instance.PlayerMoveController.InputVector = Vector2.zero;
        Dead = true;
        StartCoroutine(AfterDead());
    }
    
    private void AddToRevive()
    {
        Debug.Log("플레이어 부활");
        CurrentHp = MaxHp;
        Life = Math.Max(0, Life - 1);
        Dead = false;
    }
    #endregion

    private void resetPlayerStat()
    {
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
        
        WeaponSlotSize = playerDefaultData.DefaultWeaponSlotSize;
        PassiveItemSlotSize = playerDefaultData.DefaultPassiveItemSlotSize;
        TurretSlotSize = playerDefaultData.DefaultTurretSlotSize;
    }
    
    #region 플레이어 데미지 관련 메서드
    // 플레이어의 hp를 치료하는 효과는 다른 함수로 구현하도록 한다.
    public void TakeDamage(int damage)
    {
        if (Dead == true) return;

        _playerEventController.CallHurt();
        
        int calcDmg = CalculateReducedDmg(damage, Defense);
        CurrentHp -= calcDmg;
        Debug.Log($"{calcDmg} 적용, 남은 체력: {CurrentHp}");

        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            _playerEventController.CallDeath();
        }
    }
    int CalculateReducedDmg(int damage, int defense)
    {
        float value = damage * 100 / (100 + defense);
        return (int)Math.Round(value);
    }
    #endregion
    
    #region 플레이어 사망 관련 메서드
    private IEnumerator AfterDead()
    {
        yield return _reviveDelayAction;
        if (Life > 0) _playerEventController.CallRevive();
    }
    #endregion
}
