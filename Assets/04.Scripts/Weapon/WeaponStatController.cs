/*
무기의 스탯을 관리하는 컨트롤러 스크립트
무기 스탯 관리를 제외한 다른 요소들은 이 스크립트에서 구현해서는 안된다.
*/
using System;
using UnityEngine;

public enum WeaponStat {Level, Damage, CritRate, CritMultiplier, EffectRate, AtkSpeed, AtkRange, ProjectileSpeed, ProjectileCount}

public class WeaponStatController : MonoBehaviour, IItemStatController
{
    private WeaponEventController _weaponEventController;
    [SerializeField] private CircleCollider2D _weaponRangeCollider2D;

    private void Awake()
    {
        _weaponEventController = GetComponent<WeaponEventController>();
        _weaponRangeCollider2D = GetComponent<CircleCollider2D>();
    }

    [SerializeField] private int level;
    public int Level { get => level; set { level = value; _weaponEventController.CallOnStatChanged(WeaponStat.Level); } }

    [SerializeField] private int damage;
    public int Damage { get => damage; set { damage = value; _weaponEventController.CallOnStatChanged(WeaponStat.Damage); } }

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
    
    private void OnEnable()
    {
        _weaponEventController.OnStatChanged += ChangeColliderRadius;
    }

    private void OnDisable()
    {
        _weaponEventController.OnStatChanged -= ChangeColliderRadius;
    }

    private void ChangeColliderRadius(WeaponStat stat)
    {
        if (stat != WeaponStat.AtkRange) return;
        _weaponRangeCollider2D.radius = AtkRange;
    }
    public int GetLevel()
    {
        return level;
    }
    public void SetUp(WeaponStatData baseStat)
    {
        ResetWeaponData(baseStat);
        _weaponRangeCollider2D.radius = AtkRange;
        //AtkSpeed = baseStat.AtkSpeed;
    }

    private void ResetWeaponData(WeaponStatData baseStat)
    {
        level = baseStat.Level;
        damage = baseStat.Damage;
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

    public void LevelUp()
    {
        level++;
    }
}
