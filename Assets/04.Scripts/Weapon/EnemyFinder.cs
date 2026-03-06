using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    [Header("목표 적 레이어")]
    [SerializeField]
    private LayerMask _targetLayer;
    
    private Transform[] GetEnemiesInRange(float radius)
    {
        Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, radius, _targetLayer);
        int targetCount = targetColliders.Length;
        Transform[] detactedTargetsTransform = new Transform[targetCount];
        
        for (int i = 0; i < targetCount; i++)
        detactedTargetsTransform[i] = targetColliders[i].transform;

        return detactedTargetsTransform;
    }

    public Transform GetClosestEnemy(float radius)
    {
        Transform[] objects = GetEnemiesInRange(radius);
        // 적을 찾을 수 없거나, 찾은 적이 없는 경우 null을 반환하도록 했음.
        // 따라서 반환값이 null일 때는 적이 없다고 판단하도록 함.
        if (objects == null || objects.Length == 0) return null;

        Transform bestTarget = null;
        float closestDistance = float.MaxValue;
        Vector2 currentPosition = transform.position;

        foreach (Transform currentObject in objects)
        {
            if(currentObject == null) continue;

            Vector2 differenceToTarget = (Vector2)currentObject.position - currentPosition;
            float distanceToTarget = differenceToTarget.sqrMagnitude;
            
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                bestTarget = currentObject;
            }
        }
        return bestTarget;
    }
}
