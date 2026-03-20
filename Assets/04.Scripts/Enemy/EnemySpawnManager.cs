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
    Scout,
    Boss,
}

public class EnemySpawnManager : MonoBehaviour
{
    private List<EnemySpawnPoint> _spawnPoints;

    [SerializeField] private Wavedata _waveData;
    private int _waveIndex = -1;

    private void Start()
    {
        _spawnPoints = new List<EnemySpawnPoint>(GetComponentsInChildren<EnemySpawnPoint>());
        UpdateWave();
        InGameManager.Instance.PlayerWinAction += StopSpawnAllPoints;
    }

    private void OnDestroy()
    {
        InGameManager.Instance.PlayerWinAction -= StopSpawnAllPoints;
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
            ApplyWave(_waveData.Waves[_waveIndex]);
        }
        else if (_waveIndex == -1 && _waveData.Waves.Length > 0)
        {
            _waveIndex = 0;
            ApplyWave(_waveData.Waves[0]);
        }
    }

    private void ApplyWave(Wave wave)
    {
        StopSpawnAllPoints();

        if (wave.spawnPointConfigs == null || wave.spawnPointConfigs.Length == 0)
        {
            Debug.LogWarning("Wave에 SpawnPointConfig가 설정되지 않았습니다.");
            return;
        }

        foreach (SpawnPointConfig config in wave.spawnPointConfigs)
        {
            if (config.spawnPointIndex < 0 || config.spawnPointIndex >= _spawnPoints.Count)
            {
                Debug.LogWarning($"유효하지 않은 스폰포인트 인덱스: {config.spawnPointIndex}");
                continue;
            }

            EnemySpawnPoint spawnPoint = _spawnPoints[config.spawnPointIndex];
            float interval = config.spawnInterval > 0f ? config.spawnInterval : wave.defaultSpawnInterval;
            int count = config.spawnCount > 1 ? config.spawnCount : 1;
            spawnPoint.StartSpawn(config.enemyType, interval, count, wave.isBoss);
        }

        Debug.Log($"Wave {_waveIndex} 적용: {wave.spawnPointConfigs.Length}개 스폰포인트 활성화");
    }

    public void StopSpawnAllPoints()
    {
        foreach (var sp in _spawnPoints)
        {
            sp.StopSpawn();
        }
    }
}
