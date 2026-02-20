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
    private int _waveIndex = -1;

    private void Start()
    {
        _spawnPoints = new List<EnemySpawnPoint>(GetComponentsInChildren<EnemySpawnPoint>());
        UpdateWave();
    }

    private void Update()
    {
        UpdateWave();
    }

    private void UpdateWave()
    {
        float playTime = InGameManager.Instance.PlayTime;
        int nextWaveIndex = _waveIndex + 1;

        if (nextWaveIndex < _waveData.Waves.Length && playTime >= _waveData.Waves[nextWaveIndex].startTime)
        {
            _waveIndex = nextWaveIndex;
            _currentWave = _waveData.Waves[_waveIndex];

            Debug.Log($"Wave changed: EnemyType={_currentWave.enemyType}, SpawnInterval={_currentWave.spawnInterval}");
            StopSpawnAllPoints();
            foreach (var sp in _spawnPoints)
            {
                sp.SpawnInterval = _currentWave.spawnInterval;
            }
            StartSpawnAllPoints(_currentWave.enemyType);
        }
        else if (_waveIndex == -1 && _waveData.Waves.Length > 0)
        {
            _waveIndex = 0;
            _currentWave = _waveData.GetCurrentWave(0);
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
