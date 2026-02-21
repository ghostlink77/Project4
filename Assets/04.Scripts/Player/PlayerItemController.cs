/*
플레이어의 아이템을 관리하는 메서드
아이템을 바꾸는 메서드나 각종 아이템 업그레이드 기능은 무기 개발이 끝나고 구현하도록 함
*/
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [Header("플레이어 무기 슬롯")]
    private List<GameObject> _weaponSlot = new List<GameObject>();
    private Dictionary<string, GameObject> _weaponSlots = new Dictionary<string, GameObject>();
    [SerializeField] private int _weaponSlotMaxCount = 7;

    [Header("플레이어 패시브 아이템 슬롯")]
    private List<GameObject> _passiveItemSlot = new List<GameObject>();
    private Dictionary<string, GameObject> _passiveSlots = new Dictionary<string, GameObject>();
    [SerializeField] private int _passiveSlotMaxCount = 6;

    [Header("플레이어 포탑 슬롯")]
    private List<GameObject> _turretSlot = new List<GameObject>();
    private Dictionary<string, GameObject> _turretSlots = new Dictionary<string, GameObject>();
    [SerializeField] private int _turretSlotMaxCount = 7;

    // 초기 설정 PlayerManager에서 받아오기
    public void SetUp()
    {
        PlayerStatController statCon = PlayerManager.Instance.PlayerStatController;
        ResetItemSlots(statCon.WeaponSlotSize, statCon.PassiveItemSlotSize, statCon.TurretSlotSize);
    }

    // 아이템 슬롯 초기화하는 메서드
    void ResetItemSlots(int weaponSlotSize, int passiveSlotSize, int turretSlotSize)
    {
        InitializeSlots(_weaponSlot, weaponSlotSize);
        InitializeSlots(_passiveItemSlot, passiveSlotSize);
        InitializeSlots(_turretSlot, turretSlotSize);
        Debug.Log("플레이어 아이템 슬롯 모두 초기화됨");
    }

    // 빈 무기 슬롯이 있다면 아이템 넣는 메서드
    // 빈 무기 슬롯이 없다면 무기 교환해서 넣는 기능은 따로 구현하지 않는다.
    // 추가하려는 무기가 현재 무기 슬롯에 존재한다면 레벨만 올림.
    public void AddWeaponToSlot(WeaponStatData newWeaponData)
    {
        GameObject newWeapon = Resources.Load<GameObject>($"Weapon/{newWeaponData.WeaponName}");
        int index = GetWeaponIndexInSlot(newWeaponData);
        List<GameObject> weaponSlots = _weaponSlot;
        if (index != -1)
        {
            weaponSlots[index].GetComponent<WeaponStatController>().LevelUpWeaponLevel();
            return;
        }

        bool isEmpty = CheckEmptySlot(weaponSlots, out int emptyIndex);
        if (isEmpty == true)
        {
            weaponSlots[emptyIndex] = newWeapon;
            weaponSlots[emptyIndex].GetComponent<WeaponStatController>().LevelUpWeaponLevel();
            Debug.Log("신규 무기 추가");

        }
        else Debug.Log($"weaponSlots에 남은 자리 없음");
    }

    public void AddItemToSlot<T>(T newItemData) where T : IItemStatData
    {
        Dictionary<string, GameObject> slots = GetSlots<T>();

        if (slots.ContainsKey(newItemData.GetName()))
        {
            slots[newItemData.GetName()].GetComponent<IItemStatController>().LevelUp();
            return;
        }
        else if (CheckSlotEmptySpace<T>(slots.Count))
        {
            string path = "";
            if (typeof(T) == typeof(WeaponStatData)) path = "Weapon";
            else if (typeof(T) == typeof(PassiveStatData)) path = "Passive";
            else if (typeof(T) == typeof(TurretData)) path = "Turret";
            GameObject newWeaponPrefab = Resources.Load<GameObject>($"{path}/{newItemData.GetName()}");
            if (path == "Weapon")
            {
                GameObject newWeapon = Instantiate(newWeaponPrefab, transform);
                slots[newItemData.GetName()] = newWeapon;
            }
            else
            {
                slots[newItemData.GetName()] = newWeaponPrefab;
            }   
        }
        else Debug.Log("weaponSlots에 남은 자리 없음");
    }

    public Dictionary<string, GameObject> GetSlots<T>()
    {
        if (typeof(T) == typeof(WeaponStatData)) return _weaponSlots;
        else if (typeof(T) == typeof(PassiveStatData)) return _passiveSlots;
        else if (typeof(T) == typeof(TurretData)) return _turretSlots;
        else return null;
    }

    private bool CheckSlotEmptySpace<T>(int count)
    {
        int maxcount = 0;
        if (typeof(T) == typeof(WeaponStatData)) maxcount = _weaponSlotMaxCount;
        else if (typeof(T) == typeof(PassiveStatData)) maxcount = _passiveSlotMaxCount;
        else if (typeof(T) == typeof(TurretData)) maxcount = _turretSlotMaxCount;

        return maxcount > count;
    }

    // 빈 아이템 슬롯이 있다면 아이템 넣는 메서드
    private void AddItemToSlot(GameObject newItem)
    {
        List<GameObject> itemSlots = _passiveItemSlot;
        bool isEmpty = CheckEmptySlot(itemSlots, out int emptyIndex);
        if (isEmpty == true) itemSlots[emptyIndex] = newItem;
    }

    // 빈칸이 있는지 확인하는 메서드
    // out을 사용해 비어 있는 슬롯의 번호도 확인하도록 한다.
    private bool CheckEmptySlot(List<GameObject> itemSlot, out int emptyIndex)
    {
        emptyIndex = 0;
        for (int i = 0; i < itemSlot.Count; i++)
        {
            if (itemSlot[i] == null)
            {
                emptyIndex = i;
                return true;
            }
        }
        return false;
    }

    // 슬롯의 아이템 새로운 아이템으로 대체하는 메서드
    private void ChangeItem(List<GameObject> itemSlot, int slotNumber, GameObject newItem) => itemSlot[slotNumber] = newItem;

    // 슬롯 사이즈 맞추기
    private void InitializeSlots(List<GameObject> list, int count)
    {
        // 리스트가 작으면
        if (list.Count < count) ExpandList(list, count);
        // 리스트가 더 크면
        else if (list.Count > count) ReduceList(list,count);
    }

    // 정한 사이즈만큼 리스트 크기 늘리는 메서드
    private void ExpandList(List<GameObject> list, int count)
    {
        // Debug.Log($"현재 리스트 사이즈: {list.Count}, 필요한 사이즈: {count}");
        if (list.Count >= count) return;
        int sizeToExpand = count - list.Count;
        for (int i = 0; i < sizeToExpand; i++) list.Add(null);
    }

    // 정한 사이즈 초과하는 리스트 인덱스 모두 없애는 메서드
    private void ReduceList(List<GameObject> list, int count)
    {
        // Debug.Log($"현재 리스트 사이즈: {list.Count}, 필요한 사이즈: {count}");
        if (list.Count <= count) return;
        int sizeToDelete = list.Count - count;
        list.RemoveRange(list.Count - sizeToDelete, sizeToDelete);
    }

    // 슬롯에 추가할 무기가 현재 슬롯에 있는지, 그리고 슬롯에 있다면 레벨은 몇인지 반환함.
    public int GetItemLevelInSlot<T>(T itemData) where T : IItemStatData
    {
        Dictionary<string, GameObject> itemDict = GetSlots<T>();

        if (itemDict == null) return -1;
        if (itemDict.ContainsKey(itemData.GetName())) 
            return itemDict[itemData.GetName()].GetComponent<IItemStatController>().GetLevel();
        return -1;
    }

    // 슬롯에 추가할 무기가 현재 슬롯에 있는지, 있다면 무기의 위치를 반환. 
    public int GetWeaponIndexInSlot(WeaponStatData weaponData)
    {
        if (_weaponSlot == null) return -1;

        for (int index = 0; index < _weaponSlot.Count; index++)
        {
            if (_weaponSlot[index] == null) break;
            if (_weaponSlot[index].name == weaponData.WeaponName)
                return index;
        }
        return -1;
    }
}
