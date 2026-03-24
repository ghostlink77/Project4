using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    public List<IGameData> GameDataList { get; private set; } = new();

    [SerializeField]
    private List<WeaponStatData> _weaponDatas = new List<WeaponStatData>();
    [SerializeField]
    private List<PassiveItemData> _passiveDatas = new List<PassiveItemData>();
    [SerializeField]
    private List<TurretData> _turretDatas = new List<TurretData>();
    
    protected override void Init()
    {
        base.Init();

        GameDataList.Add(new ExpData());
        GameDataList.Add(new WeaponData());
    }

    public T GetGameData<T>() where T : class, IGameData
    {
        return GameDataList.OfType<T>().FirstOrDefault();
    }

    public void SetData()
    {
        foreach (var data in GameDataList)
        {
            Debug.Log("data.Setdata.");
            data.SetData();
        }
    }
    public T GetSelectableItem<T>(string[] selectedItemNames) where T : IItemStatData
    {
        IEnumerable<T> sourceDatas = GetSourceList<T>();

        if (sourceDatas == null) return default(T);
        
        var availableItems = sourceDatas.Where(
            data => PlayerManager.Instance.PlayerItemController.GetItemLevelInSlot<T>(data) < data.GetMaxLevel() &&
            !selectedItemNames.Contains(data.GetName())).ToList();
        if (availableItems.Count == 0) return default(T);

        int index = Random.Range(0, availableItems.Count);
        return availableItems[index];
    }

    private IEnumerable<T> GetSourceList<T>() where T : IItemStatData
    {
        if (typeof(T) == typeof(WeaponStatData)) return _weaponDatas as IEnumerable<T>;
        else if (typeof(T) == typeof(PassiveItemData)) return _passiveDatas as IEnumerable<T>;
        else if (typeof(T) == typeof(TurretData)) return _turretDatas as IEnumerable<T>;
        else return null;
    }

    // NOTE: 카테고리 구분 없이 선택 가능한 아이템을 통합 풀로 반환
    public List<IItemStatData> GetAllSelectableItems(HashSet<string> excludedNames)
    {
        var playerItemController = PlayerManager.Instance.PlayerItemController;
        var allItems = new List<IItemStatData>();

        AddSelectableItemsFromSource(_weaponDatas, allItems, playerItemController, excludedNames);
        AddSelectableItemsFromSource(_passiveDatas, allItems, playerItemController, excludedNames);
        AddSelectableItemsFromSource(_turretDatas, allItems, playerItemController, excludedNames);

        return allItems;
    }

    private void AddSelectableItemsFromSource<T>(
        List<T> sourceDatas,
        List<IItemStatData> result,
        PlayerItemController playerItemController,
        HashSet<string> excludedNames) where T : IItemStatData
    {
        foreach (T data in sourceDatas)
        {
            if (excludedNames.Contains(data.GetName())) continue;

            int currentLevel = playerItemController.GetItemLevelInSlot(data);

            if (currentLevel == -1)
            {
                // NOTE: 미보유 아이템 — 슬롯 여유가 있을 때만 선택 가능
                if (playerItemController.HasSlotSpaceForNewItem(data))
                {
                    result.Add(data);
                }
            }
            else if (currentLevel < data.GetMaxLevel())
            {
                // NOTE: 보유 중이지만 만렙 미달 — 레벨업 가능
                result.Add(data);
            }
        }
    }
}
