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
        IReadOnlyList<GameObject> items = null;

        if (type == InventoryType.Weapon) items = PlayerManager.Instance.PlayerItemController.WeaponSlot;
        else if (type == InventoryType.Passive) items = PlayerManager.Instance.PlayerItemController.PassiveSlot;
        else if (type == InventoryType.Turret) items = PlayerManager.Instance.PlayerItemController.TurretSlot;

        if (items == null) return;

        for (int index = 0; index < items.Count; index++)
        {
            if (items[index] == null) return;
            _inventorySlot[index].SetSlot(items[index].name, items[index].GetComponent<IItemStatController>().GetLevel(), "");
        }
    }
}
