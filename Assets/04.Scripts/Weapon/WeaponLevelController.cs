using System;
using UnityEngine;

public class WeaponLevelController : MonoBehaviour
{
    private WeaponStatController _weaponStatController;
    private WeaponManager _weaponManager;
    private WeaponStatData _weaponStatData;

    private void Awake()
    {
        _weaponManager = GetComponent<WeaponManager>();
        _weaponStatData = _weaponManager.BaseStat;
        _weaponStatController = GetComponent<WeaponStatController>();
    }
    
    public void LevelUp()
    {
        _weaponStatController.Level += 1;
    }
    
    public void LevelDown()
    {
        _weaponStatController.Level -= 1;
    }
    
    private void MatchStatToLevel(int level)
    {
        int statIndex = level - 1;
        _weaponStatController.Damage = _weaponStatData.Damage[statIndex];
        _weaponStatController.CritRate = _weaponStatData.CritRate[statIndex];
        _weaponStatController.CritMultiplier = _weaponStatData.CritMultiplier[statIndex];
        _weaponStatController.EffectRate = _weaponStatData.EffectRate[statIndex];
        _weaponStatController.AtkSpeed = _weaponStatData.AtkSpeed[statIndex];
        _weaponStatController.AtkRange = _weaponStatData.ProjectileSpeed[statIndex];
        _weaponStatController.ProjectileSpeed = _weaponStatData.ProjectileSpeed[statIndex];
        _weaponStatController.ProjectileCount = _weaponStatData.ProjectileCount[statIndex];
    }
}
