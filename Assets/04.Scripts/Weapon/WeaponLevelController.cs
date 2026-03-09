using System;
using UnityEngine;

public class WeaponLevelController : MonoBehaviour
{
    [SerializeField]
    WeaponStatController _weaponStatController;
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
}
