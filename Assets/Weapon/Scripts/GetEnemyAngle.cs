/*
플레이어를 기준으로 적의 방향을 구하는 스크립트
*/
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GetEnemyAngle : MonoBehaviour
{
    private List<GameObject> enemiesInRange = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemiesInRange.Add(collision.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) enemiesInRange.Remove(collision.gameObject);
    }

    Vector2 GetGameObjectPosition(GameObject gameObj)
    {
        return gameObj.transform.position;
    }
    
    // 방향 뿐만이 아니라 길이를 구할 때에도 활용하기 때문에 따로 normalize는 적용하지 않는다
    private Vector2 GetDirectionVector(Vector2 startPos, Vector2 endPos)
    {
        return endPos - startPos;
    }
    
    // 방향을 구하는 것만이 목적이기 때문에, 반드시 반환값은 normalize되어야 한다.
    /*
    결과가 정상적이지 않을 경우 Vector2.zero를 출력하도록 코드를 작성했다.
    따라서 거리가 0이라면 사격을 실행하지 않도록 하는 코드를 사격 코드에 추가해야 할 것이다.
    */
    public Vector2 FindCLosestEnemyDirection()
    {
        Vector2 playerPos = GetGameObjectPosition(gameObject);
        int length = enemiesInRange.Count;
        if (length == 0) return Vector2.zero;

        int smallestEnemyIndex = 0;
        float smallestDistance = float.MaxValue;
        
        // 적과의 거리 저장하는 리스트에 거리들 작성
        for (int i = 0; i < length; i++)
        {
            Vector2 oneEnemyDirection = GetDirectionVector(playerPos, enemiesInRange[i].transform.position);
            float oneEnemyDistance = oneEnemyDirection.sqrMagnitude;

            if (smallestDistance > oneEnemyDistance)
            {
                smallestEnemyIndex = i;
                smallestDistance = oneEnemyDistance;
            }
        }

        // 단순 거리 반환용이고, 나중에 따로 속도도 적용할 예정이기 때문에 normalize를 진행한다.
        if (enemiesInRange[smallestEnemyIndex] == null) return Vector2.zero;
        Vector2 closestEnemyPos = enemiesInRange[smallestEnemyIndex].transform.position;
        Vector2 result = GetDirectionVector(playerPos, closestEnemyPos).normalized;
        return result;
    }
}
