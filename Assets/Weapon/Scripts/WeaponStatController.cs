using NUnit.Framework;
using UnityEngine;

public class WeaponStatController : MonoBehaviour
{
    [Header("무기 기본 데이터 담은 스크립터블 오브젝트")]
    [SerializeField]
    private WeaponStatData baseStat;
    
    // 무기 범위로 사용되는 콜라이더
    private CircleCollider2D weaponRangeCollider;
    
    [SerializeField]
    private int level;
    public int Level {get => level; set => level = value;}
    [SerializeField]
    private int atk;
    public int Atk {get => atk; set => atk = value;}
    [SerializeField]
    private float critRate;
    public float CritRate {get => critRate; set => critRate = value;}
    [SerializeField]
    private float critMultiplier;
    public float CritMultiplier {get => critMultiplier; set => critMultiplier = value;}
    [SerializeField]
    private float effectRate;
    public float EffectRate {get => effectRate; set => effectRate = value;}
    [SerializeField]
    private float atkSpeed;
    public float AtkSpeed {get => atkSpeed; set => atkSpeed = value;}
    [SerializeField]
    private float atkRange;
    public float AtkRange {get => atkRange; set => atkRange = value;}
    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed {get => projectileSpeed; set => projectileSpeed = value;}
    [SerializeField]
    private float projectileCount;
    public float ProjectileCount {get => projectileCount; set => projectileCount = value;}
    
    void Start()
    {
        weaponRangeCollider = GetComponent<CircleCollider2D>();
        ResetWeaponData();
    }

    void Update()
    {
        
    }
    
    // 스크립터블 오브젝트에 저장한 대로 무기 데이터 설정 완료
    private void ResetWeaponData()
    {
        level = baseStat.Level;
        atk = baseStat.Atk;
        critRate = baseStat.CritRate;
        critMultiplier = baseStat.CritMultiplier;
        effectRate = baseStat.EffectRate;
        atkSpeed = baseStat.AtkSpeed;
        atkRange = baseStat.AtkRange;
        SyncAtkRange(atkRange);
        projectileSpeed = baseStat.ProjectileSpeed;
        projectileCount = baseStat.ProjectileCount;
    }
    
    // 매개변수로 입력된 공격 범위에 따라 조절하는 메서드
    private void SyncAtkRange(float atkRange)
    {
        weaponRangeCollider.radius = atkRange;
        if (weaponRangeCollider.radius == atkRange) Debug.Log($"콜라이더 반경 {weaponRangeCollider.radius}으로 설정됨");
    }
}
