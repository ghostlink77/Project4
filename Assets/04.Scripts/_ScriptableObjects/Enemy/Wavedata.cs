using JetBrains.Annotations;
using System;
using UnityEngine;

[Serializable]
public class Wave
{
    public float startTime;

    public EnemyType enemyType;

    public float spawnInterval;

    public Wave(float startTime, EnemyType enemyType, float spawnInterval)
    {
        this.startTime = startTime;
        this.enemyType = enemyType;
        this.spawnInterval = spawnInterval;
    }
    public Wave(Wave wave)
    {
        startTime = wave.startTime;
        enemyType = wave.enemyType;
        spawnInterval = wave.spawnInterval;
    }
}

[CreateAssetMenu(fileName = "Wavedata", menuName = "Scriptable Objects/Wavedata")]
public class Wavedata : ScriptableObject
{
    [SerializeField] private Wave[] _waves;
    public Wave[] Waves => _waves;

    private Wave _basicWave = new Wave(0, EnemyType.Drone1, 1f);

    public Wave GetCurrentWave(float playTime)
    {
        if(_waves == null || _waves.Length == 0)
        {
            Debug.LogWarning("Wavedata에 웨이브가 설정되지 않았습니다.");
            return _basicWave;
        }

        Wave wave = new Wave(_basicWave);

        for(int i = 0; i < _waves.Length; i++)
        {
            if (playTime >= _waves[i].startTime)
            {
                wave = _waves[i];
            }
            else
            {
                break;
            }
        }

        return wave;
    }
}
