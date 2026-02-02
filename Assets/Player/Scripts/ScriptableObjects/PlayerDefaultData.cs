using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDefaultData", menuName = "Scriptable Objects/PlayerDefaultData")]
public class PlayerDefaultData : ScriptableObject
{
    [Header("플레이어 기본 레벨")]
    [SerializeField] private int defaultLevel;
    public int DefaultLevel {get => defaultLevel; set => defaultLevel = value;}

    [Header("플레이어 기본 최대 HP")]
    [SerializeField] private int defaultMaxHP;
    public int DefaultMaxHP {get => defaultMaxHP; set => defaultMaxHP = value;}
    
    [Header("플레이어 기본 방어력")]
    [SerializeField] private int defaultDef;
    public int DefaultDef {get => defaultDef; set => defaultDef = value;}
    
    [Header("기본 체력 회복 속도")]
    [SerializeField] private int defaultHpGenSpeed;
    public int DefaultHpGenSpeed {get => defaultHpGenSpeed; set => defaultHpGenSpeed = value;}
    
    [Header("기본 이동 속도")]
    [SerializeField] private float defaultSpeed;
    public float DefaultSpeed {get => defaultSpeed; set => defaultSpeed = value;}
    
    [Header("기본 아이템 획득 범위")]
    [SerializeField] private float defaultReceiveRadius;
    public float DefaultReceiveRadius {get => defaultReceiveRadius; set => defaultReceiveRadius = value;}
    
    [Header("기본 플레이어 행운")]
    [SerializeField] private float defaultLuck;
    public float DefaultLuck {get => defaultLuck; set => defaultLuck = value;}
    
    [Header("기본 플레이어 성장")]
    [SerializeField] private float defaultGrowth;
    public float DefaultGrowth {get => defaultGrowth; set => defaultGrowth = value;}
    
    [Header("기본 플레이어 탐욕")]
    [SerializeField] private float defaultGreed;
    public float DefaultGreed {get => defaultGreed; set => defaultGreed = value;}
    
    [Header("기본 플레이어 저주 스탯")]
    [SerializeField] private float defaultCurse;
    public float DefaultCurse {get => defaultCurse; set => defaultCurse = value;}
    
    [Header("플레이어 목숨 개수")]
    [SerializeField] private int defaultLife;
    public int DefaultLife {get => defaultLife; set => defaultLife = value;}
    
    [Header("무기 슬롯 기본 개수")]
    [SerializeField]
    private int defaultWeaponSlotSize;
    public int DefaultWeaponSlotSize {get => defaultWeaponSlotSize; set => defaultWeaponSlotSize = value;}
    
    [Header("패시브 아이템 슬롯 기본 개수")]
    [SerializeField]
    private int defaultPassiveItemSlotSize;
    public int DefaultPassiveItemSlotSize {get => defaultPassiveItemSlotSize; set => defaultPassiveItemSlotSize = value;}
    
    [Header("포탑 슬롯 기본 개수")]
    [SerializeField]
    private int defaultTurretSlotSize;
    public int DefaultTurretSlotSize {get => defaultTurretSlotSize; set => defaultTurretSlotSize = value;}
}
