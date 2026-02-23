using System;
using UnityEngine;

public class PlayerLevelControl : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] public int currentLevel = 1;
    [SerializeField] public float currentXP = 0;
    public float requiredXP = 0;

    public event Action<int> OnLevelUp;

    private void Start()
    {
        UpdateRequiredXP();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) AddXP(1);
        if (Input.GetKeyDown(KeyCode.W)) AddXP(2);
        if (Input.GetKeyDown(KeyCode.E)) AddXP(4);
    }
#endif

    public void AddXP(float amount)
    {
        currentXP += amount;
        Debug.Log($"경험치 {amount} 휙득 (현재: {currentXP} / {requiredXP})");

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

        OnLevelUp?.Invoke(currentLevel);
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