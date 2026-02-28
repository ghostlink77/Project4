using System.Collections.Generic;
using UnityEngine;

public class TurretManager : SingletonBehaviour<TurretManager>
{
    private Dictionary<string, int> _turretLevels = new Dictionary<string, int>();

    private Dictionary<string, List<TurretBase>> _placedTurrets = new Dictionary<string, List<TurretBase>>();

    public void RegisterTurretType(string turretName)
    {
        if (!_turretLevels.ContainsKey(turretName))
        {
            _turretLevels[turretName] = 1;
            _placedTurrets[turretName] = new List<TurretBase>();
        }
    }

    public void LevelUpTurretType(string turretName)
    {
        if (!_turretLevels.ContainsKey(turretName))
        {
            Debug.LogWarning($"TurretManager: {turretName}이(가) 등록되지 않았습니다.");
            return;
        }

        _turretLevels[turretName]++;
        int newLevel = _turretLevels[turretName];

        if (_placedTurrets.TryGetValue(turretName, out List<TurretBase> turrets))
        {
            for (int i = turrets.Count - 1; i >= 0; i--)
            {
                if (turrets[i] == null)
                {
                    turrets.RemoveAt(i);
                    continue;
                }
                turrets[i].SetLevel(newLevel);
            }
        }

        Debug.Log($"TurretManager: {turretName} 레벨 → {newLevel}, 설치된 인스턴스 {turrets?.Count ?? 0}개 업데이트");
    }

    public void RegisterPlacedTurret(string turretName, TurretBase instance)
    {
        if (!_placedTurrets.ContainsKey(turretName))
        {
            _placedTurrets[turretName] = new List<TurretBase>();
        }
        _placedTurrets[turretName].Add(instance);
    }

    public void UnregisterPlacedTurret(string turretName, TurretBase instance)
    {
        if (_placedTurrets.TryGetValue(turretName, out List<TurretBase> turrets))
        {
            turrets.Remove(instance);
        }
    }

    public int GetTurretLevel(string turretName)
    {
        return _turretLevels.TryGetValue(turretName, out int level) ? level : 1;
    }
}
