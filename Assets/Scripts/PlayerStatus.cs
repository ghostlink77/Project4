using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Object/PlayerStatus", order = int.MaxValue)]
public class PlayerStatus : ScriptableObject
{
    public int currentLevel;
    public float currentExp;
    public Item[] Items = new Item[7];

}

[Serializable]
public struct Item
{
    public int ItemLevel;
    public string ItemName;
    public string Description;
}
