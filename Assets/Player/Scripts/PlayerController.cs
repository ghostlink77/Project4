using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerDefaultData playerDefaultData;
    
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetPlayerStat();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //플레이어 데이터를 스크립터블 오브젝트에 있는 걸로 초기화하는 메서드
    void resetPlayerStat()
    {
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
}
