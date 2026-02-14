/*
무기의 스탯을 관리하는 컨트롤러 스크립트
무기 스탯 관리를 제외한 다른 요소들은 이 스크립트에서 구현해서는 안된다.
*/
using System;
using UnityEngine;

public enum WeaponStat {Level, Atk, CritRate, CritMultiplier, EffectRate, AtkSpeed, AtkRange, ProjectileSpeed, ProjectileCount}

public class WeaponStatController : MonoBehaviour
{
    public event Action<WeaponStat> OnStatChanged;

    [SerializeField] private int level;
    public int Level { get => level; set { level = value; OnStatChanged?.Invoke(WeaponStat.Level); } }

    [SerializeField] private int atk;
    public int Atk { get => atk; set { atk = value; OnStatChanged?.Invoke(WeaponStat.Atk); } }

    [SerializeField] private float critRate;
    public float CritRate { get => critRate; set { critRate = value; OnStatChanged?.Invoke(WeaponStat.CritRate); } }

    [SerializeField] private float critMultiplier;
    public float CritMultiplier { get => critMultiplier; set { critMultiplier = value; OnStatChanged?.Invoke(WeaponStat.CritMultiplier); } }

    [SerializeField] private float effectRate;
    public float EffectRate { get => effectRate; set { effectRate = value; OnStatChanged?.Invoke(WeaponStat.EffectRate); } }

    [SerializeField] private float atkSpeed;
    public float AtkSpeed { get => atkSpeed; set { atkSpeed = value; OnStatChanged?.Invoke(WeaponStat.AtkSpeed); } }

    [SerializeField] private float atkRange;
    public float AtkRange { get => atkRange; set { atkRange = value; OnStatChanged?.Invoke(WeaponStat.AtkRange); } }

    [SerializeField] private float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; set { projectileSpeed = value; OnStatChanged?.Invoke(WeaponStat.ProjectileSpeed); } }

    [SerializeField] private float projectileCount;
    public float ProjectileCount { get => projectileCount; set { projectileCount = value; OnStatChanged?.Invoke(WeaponStat.ProjectileCount); } }

    // 무기 범위로 사용되는 콜라이더
    private CircleCollider2D _weaponRangeCollider;
    
    public void SetUp(WeaponStatData baseStat, CircleCollider2D weaponRange)
    {
        _weaponRangeCollider = weaponRange;
        ResetWeaponData(baseStat);
        AtkSpeed = baseStat.AtkSpeed;
    }

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
        Level++;
    }
}
