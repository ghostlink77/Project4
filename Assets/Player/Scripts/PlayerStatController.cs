/*
플레이어 캐릭터의 게임 플레이 도중의 상태 변화, 성장 등을 관리하는 스크립트
*/
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    [SerializeField] private PlayerDefaultData playerDefaultData;
    
    private int currentLevel;
    private int maxHp;
    private int currentHp;
    private int defense;
    private int hpGenSpeed;
    private float moveSpeed;
    private float itemReceiveRadius;
    private float luck;
    private float growth;
    private float greed;
    private float curse;
    private int life;

    void Awake()
    {
        resetPlayerStat();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //플레이어 데이터를 스크립터블 오브젝트에 있는 걸로 초기화하는 메서드
    void resetPlayerStat()
    {
        currentLevel = playerDefaultData.DefaultLevel;
        maxHp = playerDefaultData.DefaultMaxHP;
        currentHp = maxHp;
        defense = playerDefaultData.DefaultDef;
        hpGenSpeed = playerDefaultData.DefaultHpGenSpeed;
        moveSpeed = playerDefaultData.DefaultSpeed;
        itemReceiveRadius = playerDefaultData.DefaultReceiveRadius;
        luck = playerDefaultData.DefaultLuck;
        growth = playerDefaultData.DefaultGrowth;
        greed = playerDefaultData.DefaultGreed;
        curse = playerDefaultData.DefaultCurse;
        life = playerDefaultData.DefaultLife;
    }
    
    // 플레이어 스탯 반환 및 설정하는 함수
    public float MoveSpeed {get => moveSpeed; set => moveSpeed = value;}
}
