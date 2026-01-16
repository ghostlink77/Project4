/*
플레이어의 아이템을 관리하는 메서드
아이템을 바꾸는 메서드나 각종 아이템 업그레이드 기능은 무기 개발이 끝나고 구현하도록 함
*/
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [Header("플레이어 무기 슬롯")]
    [SerializeField]
    private List<GameObject> _playerWeaponSlot = new List<GameObject>();
    
    [Header("플레이어 패시브 아이템 슬롯")]
    [SerializeField]
    private List<GameObject> _playerPassiveItemSlot = new List<GameObject>();
    
    [Header("플레이어 포탑 슬롯")]
    [SerializeField]
    private List<GameObject> _playerTurretSlot = new List<GameObject>();

    void Start()
    {
        // 아이템 슬롯들 초기화
        InitializeSlots(_playerWeaponSlot, 4);
        InitializeSlots(_playerPassiveItemSlot, 8);
        InitializeSlots(_playerTurretSlot, 4);
    }

    void Update()
    {
        
    }
    
    // 빈 아이템 슬롯에 아이템 넣는 메서드
    public void AddItemToEmptySlot(List<GameObject> itemSlot, GameObject newItem)
    {
        bool slotEmpty = CheckEmpty(itemSlot);

        // 슬롯이 비어있다면
        if (slotEmpty == true)
        {
            for (int i = 0; i < itemSlot.Count; i++)
            {
                if (itemSlot[i] == null) ChangeItem(itemSlot, i, newItem);
            }
        }
        
        //슬롯이 비어있는데 플레이어 무기 슬롯이 아니라면
        else if (slotEmpty == false && itemSlot != _playerWeaponSlot)
        {
            // 아이템 바꿔넣을 슬롯 고르는 작업 수행
            /*
            이 부분을 어떻게 구현해야 할지 모르겠다.
            일단 UI 부분이 개발되고 나면 판단해야 할 것 같다.
            아래의 CheckItem() 메서드를 활용하면 될 것 같다.
            */
        }
    }
    
    // 아이템 슬롯이 비었는지 확인하는 메서드
    private bool CheckEmpty(List<GameObject> itemSlot)
    {
        for (int i = 0; i < itemSlot.Count; i++) if (itemSlot[i] == null) return true;
        return false;
    }
    
    // 슬롯의 아이템 새로운 아이템으로 대체하는 메서드
    private void ChangeItem(List<GameObject> itemSlot, int slotNumber, GameObject newItem)
    {
        // Debug.Log($"{nameof(itemSlot)} 리스트의 {slotNumber}번째 슬롯 비어있음, {nameof(newItem)} 넣기");
        itemSlot[slotNumber] = newItem;
    }
    
    // 슬롯 사이즈 맞추기
    private void InitializeSlots(List<GameObject> list, int count)
    {
        // Debug.Log($"{nameof(list)} 리스트 크기를 {count}로 설정");
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
