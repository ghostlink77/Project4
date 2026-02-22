using System;
using UnityEngine;

public class WeaponEventController : MonoBehaviour
{
    public event Action<WeaponStat> OnStatChanged;
    public event Action OnShoot;
    
    public event Action<GameObject> OnBulletHit;

    public void CallOnStatChanged(WeaponStat stat) => OnStatChanged?.Invoke(stat);
    public void CallOnShoot() => OnShoot?.Invoke();
}

