using UnityEngine;

[CreateAssetMenu(fileName = "BlackHoleTurretData", menuName = "Scriptable Objects/BlackHoleTurretData")]
public class BlackHoleTurretData : TurretData
{
    [Header("블랙홀 포탑 스탯")]
    [field: SerializeField] public float[] EffectCooldown { get; private set; }
}
