using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    public List<IGameData> GameDataList { get; private set; } = new();

    [SerializeField]
    private List<WeaponStatData> WeaponDatas = new List<WeaponStatData>();
    
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

    public WeaponStatData GetSelectableWeapon()
    {
        List<WeaponStatData> weaponDatas = new List<WeaponStatData>(WeaponDatas);
        while (weaponDatas.Count > 0)
        {
            int index = Random.Range(0, weaponDatas.Count);
            WeaponStatData newWeaponData = weaponDatas[index];

            const int maxLevel = 10;
            int currentWeaponLevel = PlayerManager.Instance.PlayerItemController.GetWeaponLevelInSlot(newWeaponData);

            if (currentWeaponLevel < maxLevel)
            {
                return newWeaponData;
            }

            weaponDatas.RemoveAt(index);
        }
        return null;
    }

    public T GetSelectableItem<T>(List<T> itemDataList)
    {
        List<T> itemDatas = new List<T>(itemDataList);
        while (itemDatas.Count > 0)
        {
            int index = Random.Range(0, itemDatas.Count);
            T newItemData = itemDatas[index];

            const int maxLevel = 10;
            //int currentItemLevel = PlayerManager.Instance.PlayerItemController.GetWeaponLevelInSlot(newPassiveData);

            /*if (currentPassiveLevel < maxLevel)
            {
                return newItemData;
            }*/

            itemDatas.RemoveAt(index);
        }
        return default(T);
    }
}
