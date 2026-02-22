using System;
using UnityEngine;

public class WeaponEventController : MonoBehaviour
{
    public event Action<WeaponStat> OnStatChanged;
    public event Action OnShoot;
    
    public event Action<GameObject> OnBulletGenerate, OnBulletHit, OnBulletDelete;

    public void CallOnStatChanged(WeaponStat stat) => OnStatChanged?.Invoke(stat);
    public void CallOnShoot() => OnShoot?.Invoke();
    
    public void CallOnBulletGenerate(GameObject bullet) => OnBulletGenerate?.Invoke(bullet);
    public void CallOnBulletHit(GameObject bullet) => OnBulletHit?.Invoke(bullet);
    public void CallOnBulletDelete(GameObject bullet) => OnBulletDelete?.Invoke(bullet);
}

