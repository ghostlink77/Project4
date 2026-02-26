using UnityEngine;

public abstract class TurretData : ScriptableObject, IItemStatData
{
    [SerializeField] private string turretName;
    [SerializeField] private int ID;
    [SerializeField] private Sprite _icon;

    [Header("포탑 공통 스탯")]
    [field: SerializeField] public int MaxLevel { get; private set; }
    [field: SerializeField] public int ScrapCost { get; private set; }
    [field: SerializeField] public int[] MaxHp { get; private set; }
    [field: SerializeField] public float[] Range { get; private set; }

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
        if (level > 0 && level - 1 < _upgradeDescriptions.Length)
        {
            return _upgradeDescriptions[level - 1];
        }
        else
        {
            return "업그레이드 설명이 없습니다.";
        }
    }
}
