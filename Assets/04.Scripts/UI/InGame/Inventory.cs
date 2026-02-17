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

        if (items == null) return;

        int index = 0;
        foreach(var item in items)
        {
            _inventorySlot[0].SetSlot(item.Key, item.Value.GetComponent<IItemStatController>().GetLevel(), "");
            index++;
        }   
    }
}
