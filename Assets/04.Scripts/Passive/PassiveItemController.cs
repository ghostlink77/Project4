using UnityEngine;

public class PassiveItemController : MonoBehaviour, IItemStatController
{
    // ★ 이름 변경: PassiveStatData -> PassiveItemData
    public PassiveItemData StatData { get; private set; }
    private int _currentLevel = 0;

    public int GetLevel() => _currentLevel;

    public void Initialize(PassiveItemData data)
    {
        StatData = data;
        _currentLevel = 1;

        Debug.Log($"[{StatData.GetName()}] 패시브 활성화!");
        ApplyCurrentStatToPlayer();
    }

    public void LevelUp()
    {
        if (_currentLevel < StatData.maxLevel)
        {
            _currentLevel++;
            ApplyCurrentStatToPlayer();
        }
    }

    private void ApplyCurrentStatToPlayer()
    {
        if (StatData == null || StatData.levelData.Length < _currentLevel) return;

        PassiveLevelData currentData = StatData.levelData[_currentLevel - 1];

        if (PlayerManager.Instance != null && PlayerManager.Instance.PlayerStatController != null)
        {
            PlayerManager.Instance.PlayerStatController.ApplyPassiveStat(
                StatData.passiveType, currentData.value1, currentData.value2);
        }
    }
}