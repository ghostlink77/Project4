using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatData", menuName = "Scriptable Objects/WeaponStatData")]
public class WeaponStatData : ScriptableObject, IItemStatData
{
    [Header("무기 이름")]
    [SerializeField]
    private string weaponName;
    public string WeaponName => weaponName;
    
    [Header("무기 아이콘")]
    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;

    [Header("투사체 프리팹")]
    [SerializeField]
    private GameObject projectilePrefab;
    public GameObject ProjectilePrefab => projectilePrefab;

    [Header("무기 레벨")]
    [SerializeField]
    private int level;
    public int Level => level;

    [Header("최대 레벨")]
    [SerializeField]
    private int maxLevel;
    
    [Header("무기 타입")]
    [SerializeField]
    private Game.Types.WeaponType type;
    public Game.Types.WeaponType Type => type;
    
    [Header("무기 공격력")]
    [SerializeField]
    private float[] damage = new float[WeaponLevelController._maxLevel];
    public float[] Damage => damage;
    
    [Header("무기 치명타 확률")]
    [SerializeField]
    private float[] critRate = new float[WeaponLevelController._maxLevel];
    public float[] CritRate => critRate;

    [Header("무기 치명타 배수")]
    [SerializeField]
    private float[] critMultiplier = new float[WeaponLevelController._maxLevel];
    public float[] CritMultiplier => critMultiplier;

    [Header("상태 이상 확률")]
    [SerializeField]
    private float[] effectRate = new float[WeaponLevelController._maxLevel];
    public float[] EffectRate => effectRate;

    [Header("공격 쿨타임(공격 속도)")]
    [SerializeField]
    private float[] atkSpeed = new float[WeaponLevelController._maxLevel];
    public float[] AtkSpeed => atkSpeed;

    [Header("공격 사거리")]
    [SerializeField]
    private float[] atkRange = new float[WeaponLevelController._maxLevel];
    public float[] AtkRange => atkRange;

    [Header("투사체 속도")]
    [SerializeField]
    private float[] projectileSpeed = new float[WeaponLevelController._maxLevel];
    public float[] ProjectileSpeed => projectileSpeed;
    
    [Header("투사체 개수")]
    [SerializeField]
    private int[] projectileCount = new int[WeaponLevelController._maxLevel];
    public int[] ProjectileCount => projectileCount;

    [Header("아이템 설명")]
    [SerializeField]
    private string[] _description;

    public string GetName()
    {
        return WeaponName;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public int GetMaxLevel()
    {
        return maxLevel;
    }

    public string GetDescription(int level)
    {
        return _description[level-1];
    }
}