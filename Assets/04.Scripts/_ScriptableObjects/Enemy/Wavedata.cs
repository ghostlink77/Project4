using System;
using UnityEngine;

[Serializable]
public class SpawnPointConfig
{
    public int spawnPointIndex;
    public int spawnCount = 1;
    public EnemyType enemyType;

    // NOTE: 0 이하면 Wave의 defaultSpawnInterval 사용
    public float spawnInterval;
}

[Serializable]
public class Wave
{
    public float startTime;
    public float defaultSpawnInterval = 1f;
    public SpawnPointConfig[] spawnPointConfigs;
}

[CreateAssetMenu(fileName = "Wavedata", menuName = "Scriptable Objects/Wavedata")]
public class Wavedata : ScriptableObject
{
    [SerializeField] private Wave[] _waves;
    public Wave[] Waves => _waves;
}
