using System.Collections.Generic;
using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    private List<Vector2> _enemyDirectionList;
    
    public void SetList(List<Vector2> listToSaveEnemyDirection)
    {
        _enemyDirectionList = listToSaveEnemyDirection;
    }

    void OnDisable()
    {
        _enemyDirectionList = null;
    }
}
