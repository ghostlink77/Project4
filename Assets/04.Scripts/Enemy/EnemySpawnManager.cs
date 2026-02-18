/*
 * EnemySpawnPoint들을 관리하는 매니저
 * 적의 스폰을 시작/중지하고, 스폰되는 적의 타입을 변경
 * EnemySpawnPoint들을 EnemySpawnManager 오브젝트의 자식 오브젝트로 두는 방식으로 구현
 */
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Drone1,
    Drone2,
    Robot1,
}

public class EnemySpawnManager : MonoBehaviour
{
    private List<EnemySpawnPoint> _spawnPoints;

    [SerializeField] private Wavedata _waveData;
    private Wave _currentWave;

    private void Start()
    {
        _currentWave = _waveData.GetCurrentWave(0);
        _spawnPoints = new List<EnemySpawnPoint>(GetComponentsInChildren<EnemySpawnPoint>());
        StartSpawnAllPoints(_currentWave.enemyType);
    }

    private void Update()
    {
        UpdateWave();
    }

    private void UpdateWave()
    {
        float playTime = InGameManager.Instance.PlayTime;
        Wave newWave = _waveData.GetCurrentWave(playTime);
        if (newWave != _currentWave)
        {
            _currentWave = newWave;
            Debug.Log($"Wave changed: EnemyType={_currentWave.enemyType}, SpawnInterval={_currentWave.spawnInterval}");
            StopSpawnAllPoints();
            foreach (var sp in _spawnPoints)
            {
                sp.SpawnInterval = _currentWave.spawnInterval;
            }
            StartSpawnAllPoints(_currentWave.enemyType);
        }
    }

    public void StartSpawnAllPoints(EnemyType enemyType)
    {
        foreach (var sp in _spawnPoints)
        {
            sp.StartSpawn(enemyType);
        }
    }

    public void StopSpawnAllPoints()
    {
        foreach (var sp in _spawnPoints)
        {
            sp.StopSpawn();
        }
    }
    
}
