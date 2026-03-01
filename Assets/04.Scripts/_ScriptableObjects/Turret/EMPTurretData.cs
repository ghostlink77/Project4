using UnityEngine;

[CreateAssetMenu(fileName = "EMPTurretData", menuName = "Scriptable Objects/EMPTurretData")]
public class EMPTurretData : TurretData
{
    [Header("EMP 포탑 스탯")]
    [field: SerializeField] public float[] EffectCooldown { get; private set; }
    [field: SerializeField] public float[] StunDuration { get; private set; }
}
