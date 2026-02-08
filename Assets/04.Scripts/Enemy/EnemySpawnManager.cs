/*
 * EnemySpawnPooint들을 관리하는 매니저
 * 적의 스폰을 시작/중지하고, 스폰되는 적의 타입을 변경
 * EnemySpawnPoint들을 EnemySpawnManager 오브젝트의 자식 오브젝트로 두는 방식으로 구현
 */
using NUnit.Framework;
using UnityEngine;

public class EnemySpawnManager : SingletonBehaviour<EnemySpawnManager>
{
    private EnemySpawnPoint[] spawnPoints;
    private void Start()
    {
        spawnPoints = GetComponentsInChildren<EnemySpawnPoint>();
        StartSpawnAllPoints();
    }
    public void StartSpawnAllPoints()
    {
        foreach (var sp in spawnPoints)
        {
            sp.StartSpawn();
        }
    }
    public void StopSpawnAllPoints()
    {
        foreach (var sp in spawnPoints)
        {
            sp.StopSpawn();
        }
    }

    public void ChangeSpawningEnemyType(string newEnemyType)
    {
        foreach (var sp in spawnPoints)
        {
            sp.EnemyType = newEnemyType;
        }
    }
}
