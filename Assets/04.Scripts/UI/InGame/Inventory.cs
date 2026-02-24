using System.Collections.Generic;
using UnityEngine;

public enum InventoryType
{
    Weapon,
    Passive,
    Turret
}

public class Inventory : MonoBehaviour
{
    private const int Inventory_Size = 7;
    [SerializeField] private InventoryType type;
    [SerializeField] private ItemSlot[] _inventorySlot = new ItemSlot[Inventory_Size];

    public void UpdateSlot()
    {
        Dictionary<string, GameObject> items = null;
        if (type == InventoryType.Weapon) items = PlayerManager.Instance.PlayerItemController.GetSlots<WeaponStatData>();
        else if (type == InventoryType.Passive) items = PlayerManager.Instance.PlayerItemController.GetSlots<PassiveStatData>();
        else if (type == InventoryType.Turret) items = PlayerManager.Instance.PlayerItemController.GetSlots<TurretData>();

        int index = 0;
        if (items != null)
        {
            foreach (var item in items)
            {
                if (index >= _inventorySlot.Length)
                {
                    Debug.Log("설정된 인벤토리가 초과되었습니다.");
                    break;
                }
                if (_inventorySlot[index] == null)
                {
                    Debug.Log("인벤토리 슬롯이 없습니다.");
                }
                _inventorySlot[index].SetSlot(item.Key, item.Value.GetComponent<IItemStatController>().GetLevel(), "");
                index++;
            }
        }
        
        
        for(int i = index; i < _inventorySlot.Length; i++)
        {
            if (_inventorySlot[i] == null)
            {
                Debug.Log("인벤토리 슬롯이 없습니다.");
                return;
            }
            _inventorySlot[i].ResetSlot();
        }
    }
}
