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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeLevel(_weaponStatController.Level + 1);
            Debug.LogError($"레벨 오름: {_weaponStatController.Level}");
        }
    }

    // 무기 레벨을 수정하기 위해서는 이 메서드를 호출하면 됨
    public void ChangeLevel(int level)
    {
        _weaponStatController.Level = level;
        MatchStatToLevel(level);
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
