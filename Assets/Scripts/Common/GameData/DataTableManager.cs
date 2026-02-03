using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    public List<IGameData> GameDataList { get; private set; } = new();

    
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
            data.SetData();
        }
    }
}
