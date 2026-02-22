using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Scriptable Objects/TurretData")]
public class TurretData : ScriptableObject, IItemStatData
{
    [SerializeField] private string turretName;
    [SerializeField] private Sprite _icon;
    public int scrapCost;

    public int maxHp;
    public int damage;
    public float range;
    public float fireRate;
    public float projectileSpeed;

    public string GetName()
    {
        return turretName;
    }
    public Sprite GetIcon()
    {
        return _icon;
    }
}
