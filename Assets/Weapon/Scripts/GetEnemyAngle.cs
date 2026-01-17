/*
플레이어의 하위 오브젝트로 생성된 무기의 월드 좌표, 
*/
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GetEnemyAngle : MonoBehaviour
{
    private Vector2 playerPos;
    private Vector2 targetPos;
    private GameObject targetObj;
    private CircleCollider2D weaponCollider;
    private List<GameObject> enemiesInRange = new List<GameObject>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponCollider = GetComponent<CircleCollider2D>();
        GetPlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 트리거 콜라이더에 적이 들어올 때
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemiesInRange.Add(collision.gameObject);
    }

    // 트리거 콜라이더에서 적이 나갈 때
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
            if (targetObj == collision.gameObject) targetObj = null;
        }
    }

    // 플레이어 위치 갱신하는 메서드
    void GetPlayerPosition()
    {
        playerPos = transform.position;
    }
    
    // 적 위치 갱신하는 메서드
    void GetTargetPosition()
    {
        // 타겟인 적의 게임 오브젝트 할당
        
        // 변수에 가져오기
        targetPos = targetObj.transform.position;
    }
    
    // 매개변수로 시작 지점, 끝 지점 설정한 후 벡터 반환하는 메서드
    public Vector2 GetDirectionVector(Vector2 startPos, Vector2 endPos)
    {
        return endPos - startPos;
    }
    
    // 제공된 2차원 벡터의 크기를 구하는 메서드
    public double GetDistance(Vector2 directionVector)
    {
        double distance = Math.Pow(directionVector.x, 2) + Math.Pow(directionVector.y, 2);
        return Math.Sqrt(distance);
    }
    
    public Vector2 FindCLosestEnemyDirection()
    {
        int length = enemiesInRange.Count;
        // 플레이어와 적의 거리 저장하는 리스트
        List<double> enemyDistance = new List<double>();
        
        // 플레이어 위치 동기화
        GetPlayerPosition();
        
        // 적과의 거리 저장하는 리스트에 거리들 작성
        for (int i = 0; i < length; i++)
        {
            // 플레이어에서 적으로 도달하는 벡터 구하는 배열
            enemyDistance.Add(GetDistance(GetDirectionVector(playerPos, enemiesInRange[i].transform.position)));
        }
        
        int smallestEnemyIndex = 0;
        double smallestDirection = enemyDistance[0];
        
        // 거리들 비교하고, 제일 작은 값과 인덱스 번호 찾아내기
        for (int i = 1; i < length; i++)
        {
            if (smallestDirection > enemyDistance[i])
            {
                smallestEnemyIndex = i;
                smallestDirection = enemyDistance[i];
            }
        }

        return enemiesInRange[smallestEnemyIndex].transform.position;
    }
}
