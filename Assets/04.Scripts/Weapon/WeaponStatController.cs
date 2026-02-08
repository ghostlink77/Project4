/*
무기의 스탯을 관리하는 컨트롤러 스크립트
무기 스탯 관리를 제외한 다른 요소들은 이 스크립트에서 구현해서는 안된다.
*/
using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Game.Types;
using NUnit.Framework;
using UnityEngine;

public enum WeaponStat {Level, Atk, CritRate, CritMultiplier, EffectRate, AtkSpeed, AtkRange, ProjectileSpeed, ProjectileCount}

public class WeaponStatController : MonoBehaviour
{
    public event Action<WeaponStat> OnStatChanged;

    public int Level { get { return level; } }
    [SerializeField]
    private int level, atk;
    [SerializeField]
    private float critRate, critMultiplier;
    [SerializeField]
    private float effectRate;
    [SerializeField]
    private float atkSpeed, atkRange;
    [SerializeField]
    private float projectileSpeed, projectileCount;

    // 무기 범위로 사용되는 콜라이더
    private CircleCollider2D _weaponRangeCollider;
    
    public void SetUp(WeaponStatData baseStat, CircleCollider2D weaponRange)
    {
        _weaponRangeCollider = weaponRange;
        ResetWeaponData(baseStat);
        SetStat(WeaponStat.AtkSpeed, baseStat.AtkSpeed);
    }

    // 스탯 가져올 때
    public void SetStat<T>(WeaponStat type, T value)
    {
        switch (type)
        {
            case WeaponStat.Level: level = Convert.ToInt32(value); break;
            case WeaponStat.Atk: atk = Convert.ToInt32(value); break;
            case WeaponStat.CritRate: critRate = Convert.ToSingle(value); break;
            case WeaponStat.CritMultiplier: critMultiplier = Convert.ToSingle(value); break;
            case WeaponStat.EffectRate: effectRate = Convert.ToSingle(value); break;
            case WeaponStat.AtkSpeed: atkSpeed = Convert.ToSingle(value); break;
            case WeaponStat.AtkRange: atkRange = Convert.ToSingle(value); break;
            case WeaponStat.ProjectileSpeed: projectileSpeed = Convert.ToSingle(value); break;
            case WeaponStat.ProjectileCount: projectileCount = Convert.ToSingle(value); break;
        }
        
        OnStatChanged?.Invoke(type);
    }
    
    // 스탯 읽을 때
    public float GetStat(WeaponStat type) => type switch
    {
        WeaponStat.Level => level,
        WeaponStat.Atk => atk,
        WeaponStat.CritRate => critRate,
        WeaponStat.CritMultiplier => critMultiplier,
        WeaponStat.EffectRate => effectRate,
        WeaponStat.AtkSpeed => atkSpeed,
        WeaponStat.AtkRange => atkRange,
        WeaponStat.ProjectileSpeed => projectileSpeed,
        WeaponStat.ProjectileCount => projectileCount,
        _ => 0
    };

    // 스크립터블 오브젝트에 저장한 대로 무기 데이터 설정 완료
    private void ResetWeaponData(WeaponStatData baseStat)
    {
        level = baseStat.Level;
        atk = baseStat.Atk;
        critRate = baseStat.CritRate;
        critMultiplier = baseStat.CritMultiplier;
        effectRate = baseStat.EffectRate;
        atkSpeed = baseStat.AtkSpeed;
        atkRange = baseStat.AtkRange;
        projectileSpeed = baseStat.ProjectileSpeed;
        projectileCount = baseStat.ProjectileCount;
    }

    public void LevelUpWeaponLevel()
    {
        level++;
    }
}
