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

    [SerializeField] private WeaponStatData _weaponStat;
    public static int maxLevel = 8;

    [SerializeField] private int _level;
    [SerializeField] private float _damage;
    [SerializeField] private float _critRate;
    [SerializeField] private float _critMultiplier;
    [SerializeField] private float _effectRate;
    [SerializeField] private float _atkSpeed;
    [SerializeField] private float _atkRange;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _projectileCount;

    public int Level { get => _level; set { _level = value; _weaponEventController.CallOnStatChanged(WeaponStat.Level); } }
    public float Damage { get => _damage; set { _damage = value; _weaponEventController.CallOnStatChanged(WeaponStat.Damage); } }
    public float CritRate { get => _critRate; set { _critRate = value; _weaponEventController.CallOnStatChanged(WeaponStat.CritRate); } }
    public float CritMultiplier { get => _critMultiplier; set { _critMultiplier = value; _weaponEventController.CallOnStatChanged(WeaponStat.CritMultiplier); } }
    public float EffectRate { get => _effectRate; set { _effectRate = value; _weaponEventController.CallOnStatChanged(WeaponStat.EffectRate); } }
    public float AtkSpeed { get => _atkSpeed; set { _atkSpeed = value; _weaponEventController.CallOnStatChanged(WeaponStat.AtkSpeed); } }
    public float AtkRange { get => _atkRange; set { _atkRange = value; _weaponEventController.CallOnStatChanged(WeaponStat.AtkRange); } }
    public float ProjectileSpeed { get => _projectileSpeed; set { _projectileSpeed = value; _weaponEventController.CallOnStatChanged(WeaponStat.ProjectileSpeed); } }
    public float ProjectileCount { get => _projectileCount; set { _projectileCount = value; _weaponEventController.CallOnStatChanged(WeaponStat.ProjectileCount); } }

    private void Awake()
    {
        _weaponEventController = GetComponent<WeaponEventController>();
        _weaponRangeCollider2D = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        _weaponEventController.OnStatChanged += ChangeColliderRadius;
    }

    private void OnDisable()
    {
        _weaponEventController.OnStatChanged -= ChangeColliderRadius;
    }

    public void SetUp(WeaponStatData baseStat)
    {
        _weaponStat = baseStat;
        ChangeLevel(baseStat.Level);
        _weaponRangeCollider2D.radius = AtkRange;
        //AtkSpeed = baseStat.AtkSpeed;
    }

    public int GetLevel()
    {
        return _level;
    }

    public void LevelUp()
    {
        _level++;
        ChangeLevel(_level);
    }

    private void ChangeColliderRadius(WeaponStat stat)
    {
        if (stat != WeaponStat.AtkRange) return;
        _weaponRangeCollider2D.radius = AtkRange;
    }

    private void ChangeLevel(int level)
    {
        if (level < 1)
        {
            Debug.LogWarning($"입력된 레벨 값{level}이 1보다 작음. 1로 수정함.");
            level = 1;
        }
        else if (level > maxLevel)
        {
            Debug.LogWarning($"입력되 레벨값 {level}이 8보다 큼. 8로 수정함.");
            level = maxLevel;
        }
        _level = level;
        ApplyStatToLevel(level);
    }

    private void ApplyStatToLevel(int level)
    {
        if (_weaponStat == null)
        {
            Debug.LogError("WeaponStatData 없음.");
            return;
        }
        int statIndex = level - 1;
        _damage = _weaponStat.Damage[statIndex];
        _critRate = _weaponStat.CritRate[statIndex];
        _critMultiplier = _weaponStat.CritMultiplier[statIndex];
        _effectRate = _weaponStat.EffectRate[statIndex];
        _atkSpeed = _weaponStat.AtkSpeed[statIndex];
        _atkRange = _weaponStat.AtkRange[statIndex];
        _projectileSpeed = _weaponStat.ProjectileSpeed[statIndex];
        _projectileCount = _weaponStat.ProjectileCount[statIndex];
    }
}
