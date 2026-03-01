using System;
using UnityEngine;

public class PlayerLevelControl : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] public int currentLevel = 1;
    [SerializeField] public int currentXP = 0;
    public int requiredXP = 0;

    public event Action<int> OnLevelUp;
    public event Action LevelUpEvent;

    private void Start()
    {
        UpdateRequiredXP();
    }

    public void AddXP(int amount)
    {
        float growthStat = 0;
        if (PlayerManager.Instance != null && PlayerManager.Instance.PlayerStatController != null)
        {
            growthStat = PlayerManager.Instance.PlayerStatController.Growth;
        }

        float bonusRate = 1.0f + growthStat;

        float finalAmount = amount * bonusRate;
        currentXP += finalAmount;

        Debug.Log($"[XP 획득] 기본:{amount} | 패시브 보너스:+{growthStat * 100}% | 최종획득:{finalAmount} (현재:{currentXP}/{requiredXP})");
        PlayerManager.Instance.PlayerStatController.CurrentExp = currentXP;
        InGameManager.Instance.InGameUIController.UpdateExpBar();

        while (currentXP >= requiredXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentXP -= requiredXP;
        currentLevel++;
        UpdateRequiredXP();

        Debug.Log($"LEVEL UP! 현재 레벨 {currentLevel}");

        //OnLevelUp?.Invoke(currentLevel);
        LevelUpEvent?.Invoke();
        PlayerManager.Instance.PlayerStatController.CurrentLevel = currentLevel;
    }

    private void UpdateRequiredXP()
    {
        const int MaxLevelTier1 = 15;
        const int MaxLevelTier2 = 30;
        const int MaxLevelTier3 = 50;

        int L = currentLevel;

        if (L <= MaxLevelTier1)
        {
            requiredXP = 8 * L + 10;
        }
        else if (L <= MaxLevelTier2)
        {
            requiredXP = 15 * L + 120;
        }
        else if (L <= MaxLevelTier3)
        {
            requiredXP = 30 * L + 300;
        }
        else
        {
            requiredXP = 2 * (L * L) + 500;
        }
    }
}