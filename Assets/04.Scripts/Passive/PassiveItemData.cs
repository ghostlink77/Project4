using UnityEngine;

[System.Serializable]
public struct PassiveLevelData
{
    [TextArea(2, 3)]
    public string description;
    public float value1;
    public float value2;
}

[CreateAssetMenu(fileName = "New Passive Item Data", menuName = "Scriptable Objects/PassiveItemData")]
public class PassiveItemData : ScriptableObject, IItemStatData
{
    [Header("기본 정보")]
    public string itemName;
    public Sprite icon;

    [Header("패시브 설정")]
    public Passive passiveType;
    public int maxLevel = 5;

    [Header("레벨별 데이터 (1Lv ~ 5Lv)")]
    public PassiveLevelData[] levelData;

    public string GetName() => itemName;
    public Sprite GetIcon() => icon;
    public int GetMaxLevel() => maxLevel;

    public string GetDescription(int level)
    {
        if (level > maxLevel) return "최대 레벨입니다.";
        if (levelData == null || levelData.Length < level) return "데이터가 없습니다.";

        PassiveLevelData data = levelData[level - 1];
        return string.Format(data.description, data.value1, data.value2);
    }
}

public enum Passive
{
    Heart, Nuclear, Shield, Repair, Speed, Luck, EXP, Magnet, AgitHP, AgitDef, AgitArea, TurretDmg
}