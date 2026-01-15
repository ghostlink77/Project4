using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [Header("플레이어 무기 슬롯")]
    [SerializeField]
    private List<GameObject> playerWeaponSlot = new List<GameObject>();
    
    [Header("플레이어 패시브 아이템 슬롯")]
    [SerializeField]
    private List<GameObject> playerPassiveItemSlot = new List<GameObject>();

    void Awake()
    {
        InitializeSlots(playerWeaponSlot, 4);
        InitializeSlots(playerPassiveItemSlot, 8);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    // 슬롯 사이즈 맞추기
    private void InitializeSlots(List<GameObject> list, int count)
    {
        Debug.Log($"{nameof(list)} 리스트 크기를 {count}로 설정");
        // 리스트가 작으면
        if (list.Count < count) ExpandList(list, count);
        // 리스트가 더 크면
        else if (list.Count > count) ReduceList(list,count);
    }
    
    // 정한 사이즈만큼 리스트 크기 늘리는 메서드
    private void ExpandList(List<GameObject> list, int count)
    {
        Debug.Log($"현재 리스트 사이즈: {list.Count}, 필요한 사이즈: {count}");
        if (list.Count >= count) return;
        int sizeToExpand = count - list.Count;
        for (int i = 0; i < sizeToExpand; i++) list.Add(null);
    }
    
    // 정한 사이즈 초과하는 리스트 인덱스 모두 없애는 메서드
    private void ReduceList(List<GameObject> list, int count)
    {
        Debug.Log($"현재 리스트 사이즈: {list.Count}, 필요한 사이즈: {count}");
        if (list.Count <= count) return;
        int sizeToDelete = list.Count - count;
        list.RemoveRange(list.Count - sizeToDelete, sizeToDelete);
    }
}
