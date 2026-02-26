using UnityEngine;

public abstract class TurretData : ScriptableObject, IItemStatData
{
    [SerializeField] private string turretName;
    [SerializeField] private int ID;
    [SerializeField] private Sprite _icon;

    public int MaxLevel { get => maxLevel; }
    public int ScrapCost { get => scrapCost; }

    [Header("포탑 공통 스탯")]
    [SerializeField] private int maxLevel;
    [SerializeField] private int scrapCost;
    public int[] maxHp;
    public float[] range;

    [Header("포탑 업그레이드 설명")]
    [TextArea]
    [SerializeField] private string[] _upgradeDescriptions;

    public string GetName()
    {
        return turretName;
    }
    public Sprite GetIcon()
    {
        return _icon;
    }
    public string GetUpgradeDescription(int level)
    {
        if (level - 1 < _upgradeDescriptions.Length)
        {
            return _upgradeDescriptions[level - 1];
        }
        else
        {
            return "업그레이드 설명이 없습니다.";
        }
    }
}
