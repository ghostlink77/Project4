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

    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeLevel(_weaponStatController.Level + 1);
            Debug.LogError($"레벨 오름: {_weaponStatController.Level}");
        }
    }
    #endif

    // 무기 레벨을 수정하기 위해서는 이 메서드를 호출하면 됨
    public void ChangeLevel(int level)
    {
        if (level < 1)
        {
            Debug.LogError($"입력된 레벨 값{level}이 1보다 작음. 1로 수정함.");
            level = 1;
        }
        else if (level > 8)
        {
            Debug.LogError($"입력되 레벨값 {level}이 8보다 큼. 8로 수정함.");
            level = 8;
        }
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
