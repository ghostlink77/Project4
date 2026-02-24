using UnityEngine;

public enum Passive
{
    Heart,          // 최대 HP 증가
    Nuclear,        // 공격력 증가
    Shield,         // 방어력 증가
    Repair,         // 회복력 강화
    Speed,          // 이동 속도 증가
    Luck,           // 행운 증가 (치명타, 드랍률)
    EXP,            // 경험치 획득량 증가
    Magnet,         // 수집 범위 증가
    AgitHP,         // 아지트 HP 증가
    AgitDef,        // 아지트 방어력 증가
    AgitArea,       // 아지트 영역 확장
    TurretDmg       // 포탑 공격력 증가
}

[CreateAssetMenu(fileName = "New Passive Data", menuName = "Game Data/Passive Item")]
public class LevelUpPassive : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    [TextArea] public string descriptionDesc;

    [Header("Stats")]
    public Passive passive;
    public int maxLevel = 5;

    public float[] valuePerLevel;
    public float[] valuePerLevel2;

    public string GetDescription(int nextLevel)
    {
        if (nextLevel > maxLevel) return "Max Level";

        float val1 = valuePerLevel.Length >= nextLevel ? valuePerLevel[nextLevel - 1] : 0;
        float val2 = valuePerLevel2.Length >= nextLevel ? valuePerLevel2[nextLevel - 1] : 0;

        return string.Format(descriptionDesc, val1, val2);
    }
}