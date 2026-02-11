/*
 * EnemySpawnPooint들을 관리하는 매니저
 * 적의 스폰을 시작/중지하고, 스폰되는 적의 타입을 변경
 * EnemySpawnPoint들을 EnemySpawnManager 오브젝트의 자식 오브젝트로 두는 방식으로 구현
 */
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Drone1,
    Drone2,
}

public class EnemySpawnManager : SingletonBehaviour<EnemySpawnManager>
{
    private List<EnemySpawnPoint> _spawnPoints;
    private void Start()
    {
        _spawnPoints = new List<EnemySpawnPoint>(GetComponentsInChildren<EnemySpawnPoint>());
        StartSpawnAllPoints();
    }
    public void StartSpawnAllPoints()
    {
        foreach (var sp in _spawnPoints)
        {
            sp.StartSpawn();
        }
    }
    public void StopSpawnAllPoints()
    {
        foreach (var sp in _spawnPoints)
        {
            sp.StopSpawn();
        }
    }

    public void ChangeSpawningEnemyType(EnemyType newEnemyType)
    {
        foreach (var sp in _spawnPoints)
        {
            sp.EnemyType = newEnemyType;
        }
    }
}
