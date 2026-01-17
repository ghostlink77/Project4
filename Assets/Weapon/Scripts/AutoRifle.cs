using UnityEngine;

public class AutoRifle : MonoBehaviour
{
    [Header("무기 데이터 스크립터블 오브젝트")]
    [SerializeField]
    private WeaponStatData weaponStat;
    
    // 플레이어(무기 위치)
    public Vector2 WeaponPos => transform.position;
    
    // 가장 가까운 적 위치
    private Vector2 closestEnemyPos;
    public Vector2 ClosestEnemyPos => closestEnemyPos;
    
    // 사정거리 안의 적들 담는 콜라이더

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 가장 가까운 적을 찾는 메서드
    void FindClosestEnemy()
    {
    }
}
