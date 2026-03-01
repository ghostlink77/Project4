/*
플레이어 캐릭터의 게임 플레이 도중의 상태 변화, 성장 등을 관리하는 스크립트
*/
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerStatController : MonoBehaviour, IDamageable
{
    #region 스크립터블 오브젝트
    [Header("스크립터블 오브젝트들")]
    [SerializeField] private PlayerDefaultData playerDefaultData;
    #endregion

    #region 플레이어 스탯 변수들
    public Dictionary<Passive, int> passiveLevels = new Dictionary<Passive, int>();

    public int CurrentLevel { get; set; }
    public float CurrentExp { get; set; }
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }
    public int Defense { get; set; }
    public int HpGenSpeed { get; set; }

    public float DamageMultiplier { get; set; } = 1.0f;
    public float CriticalPercent { get; set; } = 0f;

    public float MoveSpeed { get; set; }
    public float ItemGetRadius { get; set; }
    public float Luck { get; set; }
    public float Growth { get; set; }
    public float Greed { get; set; }
    public float Curse { get; set; }
    public int Life { get; set; }
    public bool Dead { get; set; }

    public int WeaponSlotSize { get; set; }
    public int PassiveItemSlotSize { get; set; }
    public int TurretSlotSize { get; set; }

    public float AgitHp { get; set; }
    public float AgitDef { get; set; }
    public int AgitArea { get; set; }
    public float TurretDmg { get; set; }

    public WeaponStatData DefaultWeapon { get => playerDefaultData.DefaultWepon; }
    #endregion

    #region 플레이어 부활 시간 관련 변수
    [SerializeField] private float _reviveDelayTime = 1.5f;
    private float ReviveDelayTime
    {
        get => _reviveDelayTime;
        set => _reviveDelayTime = (value <= 0) ? 0 : value;
    }
    private WaitForSeconds _reviveDelayAction;
    #endregion

    private PlayerEventController _playerEventController;

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
        if (PlayerManager.Instance != null)
        {
            _playerEventController = PlayerManager.Instance.PlayerEventController;
        }
        resetPlayerStat();
        _reviveDelayAction = new WaitForSeconds(ReviveDelayTime);

        StartCoroutine(HpRegenRoutine());
    }
    private void AddToEvent()
    {
        if (_playerEventController == null) return;
        _playerEventController.Death += AddToDeath;
        _playerEventController.Revive += AddToRevive;
    }

    private void RemoveFromEvent()
    {
        if (_playerEventController == null) return;
        _playerEventController.Death -= AddToDeath;
        _playerEventController.Revive -= AddToRevive;
    }

    private void AddToDeath()
    {
        if (PlayerManager.Instance.PlayerMoveController != null)
        {
            PlayerManager.Instance.PlayerMoveController.InputVector = Vector2.zero;
        }
        Dead = true;
        StartCoroutine(AfterDead());
    }

    private void AddToRevive()
    {
        CurrentHp = MaxHp;
        Life = Math.Max(0, Life - 1);
        Dead = false;
    }

    private void resetPlayerStat()
    {
        passiveLevels.Clear();

        CurrentLevel = playerDefaultData.DefaultLevel;
        MaxHp = playerDefaultData.DefaultMaxHP;
        CurrentHp = MaxHp;
        Defense = playerDefaultData.DefaultDef;
        HpGenSpeed = playerDefaultData.DefaultHpGenSpeed;
        MoveSpeed = playerDefaultData.DefaultSpeed;
        ItemGetRadius = playerDefaultData.DefaultReceiveRadius;
        Luck = playerDefaultData.DefaultLuck;
        Growth = playerDefaultData.DefaultGrowth;
        Greed = playerDefaultData.DefaultGreed;
        Curse = playerDefaultData.DefaultCurse;
        Life = playerDefaultData.DefaultLife;

        DamageMultiplier = 1.0f;
        CriticalPercent = 0f;

        AgitHp = 0f;
        AgitDef = 0f;
        AgitArea = 0;
        TurretDmg = 0f;

        WeaponSlotSize = playerDefaultData.DefaultWeaponSlotSize;
        PassiveItemSlotSize = playerDefaultData.DefaultPassiveItemSlotSize;
        TurretSlotSize = playerDefaultData.DefaultTurretSlotSize;
    }

    public int GetCurrentPassiveLevel(Passive type)
    {
        return passiveLevels.ContainsKey(type) ? passiveLevels[type] : 0;
    }

    public void LevelUpPassiveStat(PassiveItemData data)
    {
        if (!passiveLevels.ContainsKey(data.passiveType))
        {
            passiveLevels[data.passiveType] = 1;
        }
        else
        {
            passiveLevels[data.passiveType]++;
        }

        int currentLevel = passiveLevels[data.passiveType];

        float val1 = 0;
        float val2 = 0;

        if (currentLevel == 1)
        {
            val1 = (data.levelData != null && data.levelData.Length >= 1) ? data.levelData[0].value1 : 0;
            val2 = (data.levelData != null && data.levelData.Length >= 1) ? data.levelData[0].value2 : 0;
        }
        else
        {
            if (data.levelData != null && data.levelData.Length >= currentLevel)
            {
                val1 = data.levelData[currentLevel - 1].value1 - data.levelData[currentLevel - 2].value1;
                val2 = data.levelData[currentLevel - 1].value2 - data.levelData[currentLevel - 2].value2;
            }
        }

        ApplyPassiveStat(data.passiveType, val1, val2);

        Debug.Log($"[패시브 업그레이드] {data.itemName} Lv.{currentLevel} 달성! (증가량: {val1})");
    }

    public void ApplyPassiveStat(Passive type, float val1, float val2)
    {
        switch (type)
        {
            case Passive.Heart:
                int prevMaxHp = MaxHp;
                MaxHp += (int)val1;
                CurrentHp += (MaxHp - prevMaxHp);
                break;
            case Passive.Nuclear:
                DamageMultiplier += (val1 / 100f);
                break;
            case Passive.Shield:
                Defense += (int)val1;
                break;
            case Passive.Repair:
                HpGenSpeed += (int)val1;
                break;
            case Passive.Speed:
                MoveSpeed += playerDefaultData.DefaultSpeed * (val1 / 100f);
                break;
            case Passive.Luck:
                CriticalPercent += (val1 / 100f);
                Luck += (val2 / 100f);
                break;
            case Passive.EXP:
                Growth += (val1 / 100f);
                break;
            case Passive.Magnet:
                ItemGetRadius += playerDefaultData.DefaultReceiveRadius * (val1 / 100f);
                break;
            case Passive.AgitHP:
                AgitHp += val1;
                break;
            case Passive.AgitDef:
                AgitDef += val1;
                break;
            case Passive.AgitArea:
                AgitArea += (int)val1;
                break;
            case Passive.TurretDmg:
                TurretDmg += (val1 / 100f);
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (Dead == true) return;

        if (_playerEventController != null)
            _playerEventController.CallHurt();

        int calcDmg = CalculateReducedDmg(damage, Defense);
        CurrentHp -= calcDmg;
        InGameManager.Instance.InGameUIController.UpdatePlayerHpBar();

        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            if (_playerEventController != null)
                _playerEventController.CallDeath();
        }
        Debug.Log($"데미지: {damage}, 현재 hp: {CurrentHp}");
    }

    int CalculateReducedDmg(int damage, int defense)
    {
        float value = damage * 100.0f / (100.0f + defense);
        return (int)Math.Round(value);
    }

    private IEnumerator AfterDead()
    {
        yield return _reviveDelayAction;
        if (Life > 0 && _playerEventController != null)
        {
            _playerEventController.CallRevive();
        }
    }

    private IEnumerator HpRegenRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (Dead == false && HpGenSpeed > 0 && CurrentHp < MaxHp)
            {
                CurrentHp += HpGenSpeed;
                InGameManager.Instance.InGameUIController.UpdatePlayerHpBar();

                if (CurrentHp > MaxHp)
                {
                    CurrentHp = MaxHp;
                }
            }
        }
    }
}