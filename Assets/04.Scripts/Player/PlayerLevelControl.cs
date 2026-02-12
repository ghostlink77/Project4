using UnityEngine;

public class PlayerLevelControl : MonoBehaviour
{
    [Header("Player Status")]
    public int currentLevel = 1;
    public float currentXP = 0;
    public float requiredXP = 0;

    void Start()
    {
        UpdateRequiredXP();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) AddXP(1);
        if (Input.GetKeyDown(KeyCode.W)) AddXP(2);
        if (Input.GetKeyDown(KeyCode.E)) AddXP(4);
    }

    public void AddXP(float amount)
    {
        currentXP += amount;
        Debug.Log($"경험치 {amount} 획득 (현재: {currentXP} / {requiredXP})");

        while (currentXP >= requiredXP)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentXP -= requiredXP;
        currentLevel++;
        UpdateRequiredXP();

        Debug.Log($"LEVEL UP! 현재 레벨: {currentLevel}");

        if (InGameManager.Instance != null)
        {
            InGameManager.Instance.OpenLevelUpUI();
        }
        else
        {
            Debug.LogError("InGameManager가 씬에 없습니다!");
        }
    }

    void UpdateRequiredXP()
    {
        int L = currentLevel;

        if (L >= 1 && L <= 15)
        {
            requiredXP = 8 * L + 10;
        }
        else if (L >= 16 && L <= 30)
        {
            requiredXP = 15 * L + 120;
        }
        else if (L >= 31 && L <= 50)
        {
            requiredXP = 30 * L + 300;
        }
        else
        {
            requiredXP = 2 * (L * L) + 500;
        }
    }
}