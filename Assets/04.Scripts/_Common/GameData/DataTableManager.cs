using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    public List<IGameData> GameDataList { get; private set; } = new();

    [SerializeField]
    private List<WeaponStatData> _weaponDatas = new List<WeaponStatData>();
    [SerializeField]
    private List<PassiveStatData> _passiveDatas = new List<PassiveStatData>();
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
    public T GetSelectableItem<T>() where T : IItemStatData
    {
        IEnumerable<T> sourceDatas = GetSourceList<T>();

        if (sourceDatas == null) return default(T);

        const int maxLevel = 10;
        var availableItems = sourceDatas.Where(data => PlayerManager.Instance.PlayerItemController.GetItemLevelInSlot<T>(data) <= maxLevel).ToList();
        if (availableItems.Count == 0) return default(T);

        int index = Random.Range(0, availableItems.Count);
        return availableItems[index];
    }

    private IEnumerable<T> GetSourceList<T>() where T : IItemStatData
    {
        if (typeof(T) == typeof(WeaponStatData)) return _weaponDatas as IEnumerable<T>;
        else if (typeof(T) == typeof(PassiveStatData)) return _passiveDatas as IEnumerable<T>;
        else if (typeof(T) == typeof(TurretData)) return _turretDatas as IEnumerable<T>;
        else return null;
    }


}
