using JetBrains.Annotations;
using System;
using UnityEngine;

[Serializable]
public class Wave
{
    public float startTime;

    public EnemyType enemyType;

    public float spawnInterval;
}

[CreateAssetMenu(fileName = "Wavedata", menuName = "Scriptable Objects/Wavedata")]
public class Wavedata : ScriptableObject
{
    [SerializeField] private Wave[] _waves;
    public Wave[] Waves => _waves;

    public Wave GetCurrentWave(float playTime)
    {
        Wave wave = null;

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
