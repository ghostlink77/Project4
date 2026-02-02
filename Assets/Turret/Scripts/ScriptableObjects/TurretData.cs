using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Scriptable Objects/TurretData")]
public class TurretData : ScriptableObject
{
    public string turretName;
    public GameObject turretPrefab;
    public SpriteRenderer icon;
    public int scrapCost;

    public int damage;
    public float range;
    public float fireRate;
    public float projectileSpeed;
}
