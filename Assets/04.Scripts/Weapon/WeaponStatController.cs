/*
무기의 스탯을 관리하는 컨트롤러 스크립트
무기 스탯 관리를 제외한 다른 요소들은 이 스크립트에서 구현해서는 안된다.
*/
using System;
using UnityEngine;

public enum WeaponStat {Level, Atk, CritRate, CritMultiplier, EffectRate, AtkSpeed, AtkRange, ProjectileSpeed, ProjectileCount}

public class WeaponStatController : MonoBehaviour, IItemStatController
{
    private WeaponEventController _weaponEventController;

    private void Awake()
    {
        _weaponEventController = GetComponent<WeaponEventController>();
    }

    [SerializeField] private int level;
    public int Level { get => level; set { level = value; _weaponEventController.CallOnStatChanged(WeaponStat.Level); } }

    [SerializeField] private int atk;
    public int Atk { get => atk; set { atk = value; _weaponEventController.CallOnStatChanged(WeaponStat.Atk); } }

    [SerializeField] private float critRate;
    public float CritRate { get => critRate; set { critRate = value; _weaponEventController.CallOnStatChanged(WeaponStat.CritRate); } }

    [SerializeField] private float critMultiplier;
    public float CritMultiplier { get => critMultiplier; set { critMultiplier = value; _weaponEventController.CallOnStatChanged(WeaponStat.CritMultiplier); } }

    [SerializeField] private float effectRate;
    public float EffectRate { get => effectRate; set { effectRate = value; _weaponEventController.CallOnStatChanged(WeaponStat.EffectRate); } }

    [SerializeField] private float atkSpeed;
    public float AtkSpeed { get => atkSpeed; set { atkSpeed = value; _weaponEventController.CallOnStatChanged(WeaponStat.AtkSpeed); } }

    [SerializeField] private float atkRange;
    public float AtkRange { get => atkRange; set { atkRange = value; _weaponEventController.CallOnStatChanged(WeaponStat.AtkRange); } }

    [SerializeField] private float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; set { projectileSpeed = value; _weaponEventController.CallOnStatChanged(WeaponStat.ProjectileSpeed); } }

    [SerializeField] private float projectileCount;
    public float ProjectileCount { get => projectileCount; set { projectileCount = value; _weaponEventController.CallOnStatChanged(WeaponStat.ProjectileCount); } }

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
