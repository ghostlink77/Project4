/*
플레이어 캐릭터의 게임 플레이 도중의 상태 변화, 성장 등을 관리하는 스크립트
*/
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{
    [Header("스크립터블 오브젝트들")]
    [SerializeField]
    private PlayerDefaultData playerDefaultData;
    [SerializeField]
    private SoundData playerSound;
    
    // 플레이어 스탯
    [Header("플레이어 스탯")]
    [SerializeField]
    private int currentLevel;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private int defense;
    [SerializeField]
    private int hpGenSpeed;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float itemReceiveRadius;
    [SerializeField]
    private float luck;
    [SerializeField]
    private float growth;
    [SerializeField]
    private float greed;
    [SerializeField]
    private float curse;
    [SerializeField]
    private int life;

    private Animator animator;
    private SoundManager soundManager;
    
    void Awake()
    {
        resetPlayerStat();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        soundManager = SoundManager.Instance;
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
    
    //플레이어 hp에 데미지 가하는 함수
    // 플레이어의 hp를 치료하는 효과는 다른 함수로 구현하도록 한다.
    public void GetDamage(int damage)
    {
        Debug.Log($"animator: {animator.name}, has controller: {animator.runtimeAnimatorController != null}");
        bool isDead = animator.GetBool("isDead");

        // 플레이어가 이미 죽은 경우 함수 미적용
        if (isDead) return;
        
        // 플레이어 피격 이펙트 출력
        StartPlayerAnimation();

        // 방어력 적용해서 데미지 적용
        float value = damage * 100 / (100 + defense);
        int finalDamage = (int)Math.Round(value);
        currentHp -= finalDamage;
        Debug.Log($"{finalDamage} 적용, 남은 체력: {currentHp}");

        if (currentHp <= 0)
        {
            currentHp = 0;
            SetPlayerDead();
        }
    }
    
    void StartPlayerAnimation()
    {
        animator.SetTrigger("isHurt");
        soundManager.PlayPlayerSFX(playerSound.GetClip(0));
    }
    
    //플레이어 죽음 상태 설정하는 함수
    private void SetPlayerDead()
    {
        animator.SetBool("isDead", true);
    }
    
    // 플레이어 스탯 반환 및 설정하는 함수
    public float MoveSpeed {get => moveSpeed; set => moveSpeed = value;}
}
