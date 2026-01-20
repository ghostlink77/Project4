using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Scriptable Objects/TurretData")]
public class TurretData : ScriptableObject
{
    public string turretName;
    public GameObject turretPrefab;
    public int scrapCost;

    public float damage;
    public float range;
    public float fireRate;
}
