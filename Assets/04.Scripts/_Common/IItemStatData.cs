using UnityEngine;

public interface IItemStatData
{
    public string GetName();
    public Sprite GetIcon();
    public string GetDescription(int level);
    public int GetMaxLevel();
}

