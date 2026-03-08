using System;
using UnityEngine;

public class WeaponLevelController : MonoBehaviour
{
    [SerializeField]
    WeaponStatController _weaponStatController;

    private void Awake()
    {
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
