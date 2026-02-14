using System;
using UnityEngine;

public enum WeaponStat { Level, Atk, CritRate, CritMultiplier, EffectRate, AtkSpeed, AtkRange, ProjectileSpeed, ProjectileCount }

public class WeaponEventController : MonoBehaviour
{
    public event Action<WeaponStat> OnStatChanged;
    public event Action OnShoot;

    public void CallOnStatChanged(WeaponStat stat) => OnStatChanged?.Invoke(stat);
    public void CallOnShoot() => OnShoot?.Invoke();
}

