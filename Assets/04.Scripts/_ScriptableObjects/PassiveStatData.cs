using UnityEngine;

[CreateAssetMenu(fileName = "PassiveStatData", menuName = "Scriptable Objects/PassiveStatData")]
public class PassiveStatData : ScriptableObject, IItemStatData
{
    [Header("이름")]
    [SerializeField]
    private string _passiveName;
    public string PassiveName => _passiveName;

    [Header("터렛 아이콘")]
    [SerializeField]
    private Sprite _icon;
    public Sprite Icon => _icon;

    [Header("투사체 프리팹")]
    [SerializeField]
    private GameObject projectilePrefab;
    public GameObject ProjectilePrefab => projectilePrefab;

    [Header("터렛 레벨")]
    [SerializeField]
    private int level;
    public int Level => level;

    [Header("터렛 타입")]
    [SerializeField]
    private Game.Types.WeaponType type;
    public Game.Types.WeaponType Type => type;

    [Header("터렛 공격력")]
    [SerializeField]
    private int atk;
    public int Atk => atk;

    [Header("터렛 치명타 확률")]
    [SerializeField]
    private float critRate;
    public float CritRate => critRate;

    [Header("터렛 치명타 배수")]
    [SerializeField]
    private float critMultiplier;
    public float CritMultiplier => critMultiplier;

    [Header("상태 이상 확률")]
    [SerializeField]
    private float effectRate;
    public float EffectRate => effectRate;

    [Header("공격 쿨타임(공격 속도)")]
    [SerializeField]
    private float atkSpeed;
    public float AtkSpeed => atkSpeed;

    [Header("공격 사거리")]
    [SerializeField]
    private float atkRange;
    public float AtkRange => atkRange;

    [Header("투사체 속도")]
    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed;

    [Header("투사체 개수")]
    [SerializeField]
    private int projectileCount;
    public int ProjectileCount => projectileCount;

    public string GetName()
    {
        return _passiveName;
    }

    public Sprite GetIcon()
    {
        return _icon;
    }
}
