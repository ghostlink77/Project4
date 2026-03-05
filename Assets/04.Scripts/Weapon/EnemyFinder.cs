using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    private List<GameObject> _enemyDirectionList;
    
    [Header("목표 적 레이어")]
    [SerializeField]
    private LayerMask _targetLayer;

    public void SetList(List<GameObject> listToSaveEnemyDirection)
    {
        _enemyDirectionList = listToSaveEnemyDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _targetLayer) != 0)
        {
            _enemyDirectionList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _targetLayer) != 0)
        {
            _enemyDirectionList.Remove(collision.gameObject);
        }
    }
    
    public Vector2 GetClosestEnemy()
    {
        int enemyCount = _enemyDirectionList.Count;
        if (enemyCount == 0) return Vector2.zero;

        Vector2 bulletPos = gameObject.transform.position;
        Vector2 enemyPos = _enemyDirectionList[0].transform.position;
        Vector2 direction = GetDirection(bulletPos, enemyPos);
        float closestMultipleLength = GetMultipleLength(direction);
        Vector2 closestDirection = direction;

        for (int i = 1; i < enemyCount; i++)
        {
            enemyPos = _enemyDirectionList[i].transform.position;
            direction = GetDirection(bulletPos, enemyPos);
            float currentMultipleLength = GetMultipleLength(direction);
            if (currentMultipleLength < closestMultipleLength) closestDirection = direction;
        }
        return closestDirection.normalized;
    }
    
    private Vector2 GetDirection(Vector2 start, Vector2 end)
    {
        return end - start;
    }
    private float GetMultipleLength(Vector2 direction)
    {
        return direction.x * direction.x + direction.y * direction.y;
    }

    void OnDisable()
    {
        _enemyDirectionList = null;
    }
}
