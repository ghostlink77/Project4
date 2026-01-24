/*
플레이어의 아이템을 관리하는 메서드
아이템을 바꾸는 메서드나 각종 아이템 업그레이드 기능은 무기 개발이 끝나고 구현하도록 함
*/
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    // 참조 클래스
    private PlayerManager _playerManager;

    [Header("플레이어 무기 슬롯")]
    public List<GameObject> _weaponSlot = new List<GameObject>();
    
    [Header("플레이어 패시브 아이템 슬롯")]
    public List<GameObject> _passiveItemSlot = new List<GameObject>();
    
    [Header("플레이어 포탑 슬롯")]
    public List<GameObject> _turretSlot = new List<GameObject>();

    void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    void Start()
    {
        ResetItemSlots();
    }
    
    // 아이템 슬롯 초기화하는 메서드
    void ResetItemSlots()
    {
        int weaponSlotSize = _playerManager._playerStatController.WeaponSlotSize;
        int passiveItemSlotSize = _playerManager._playerStatController.PassiveItemSlotSize;
        int turretSlotSize = _playerManager._playerStatController.TurretSlotSize;

        InitializeSlots(_weaponSlot, weaponSlotSize);
        InitializeSlots(_passiveItemSlot, passiveItemSlotSize);
        InitializeSlots(_turretSlot, turretSlotSize);
    }
    
    // 빈 무기 슬롯이 있다면 아이템 넣는 메서드
    // 빈 무기 슬롯이 없다면 무기 교환해서 넣는 기능은 따로 구현하지 않는다.
    public void AddWeaponToSlot(GameObject newWeapon)
    {
        List<GameObject> weaponSlots = _weaponSlot;
        bool isEmpty = CheckEmptySlot(weaponSlots, out int emptyIndex);
        if (isEmpty == true) weaponSlots[emptyIndex] = newWeapon;
        else Debug.Log($"weaponSlots에 남은 자리 없음");
    }
    
    // 빈 아이템 슬롯이 있다면 아이템 넣는 메서드
    // 빈 아이템 슬롯이 없다면 아이템 교환하는 기능 구현 필요
    public void AddItemToSlot(GameObject newItem)
    {
        List<GameObject> itemSlots = _passiveItemSlot;
        bool isEmpty = CheckEmptySlot(itemSlots, out int emptyIndex);
        if (isEmpty == true) itemSlots[emptyIndex] = newItem;
        // else 아이템 교체 구현
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
}
