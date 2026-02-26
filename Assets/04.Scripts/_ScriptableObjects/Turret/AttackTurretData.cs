using UnityEngine;

[CreateAssetMenu(fileName = "AttackTurretData", menuName = "Scriptable Objects/AttackTurretData")]
public class AttackTurretData : TurretData
{
    [Header("공격형 포탑 스탯")]
    [SerializeField] private string projectileAdrsKey;
    [field: SerializeField] public int[] Damage { get; private set; }
    [field: SerializeField] public int[] NumProjectile { get; private set; }
    [field: SerializeField] public float[] FireRate { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }

    public string GetProjectileKey()
    {
        return projectileAdrsKey;
    }
}
