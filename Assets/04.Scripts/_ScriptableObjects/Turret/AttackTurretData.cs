using UnityEngine;

[CreateAssetMenu(fileName = "AttackTurretData", menuName = "Scriptable Objects/AttackTurretData")]
public class AttackTurretData : TurretData
{
    [Header("공격형 포탑 스탯")]
    [SerializeField] private string projectileAdrsKey;
    public int[] damage;
    public int[] numProjectile;
    public float[] fireRate;
    public float projectileSpeed;

    public string GetProjectileKey()
    {
        return projectileAdrsKey;
    }
}
