using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    protected SOInventory _usableInventorySO;
    [SerializeField]
    protected SOInventory _equipmentInventorySO;
    [SerializeField]
    protected SOInventory _craftingInventorySO;

    public virtual void AddItems(SOItem item, int amount)
    {
        if (item.GetType() == typeof(SOUsableItem))
        {
            _usableInventorySO.AddItems(item, amount);
        }
        else if (item.GetType() == typeof(SOEquipmentItem))
        {
            _equipmentInventorySO.AddItems(item, amount);
        }
        else if (item.GetType() == typeof(SOCraftingItem))
        {
            _craftingInventorySO.AddItems(item, amount);
        }
    }

    public virtual void RemoveItems(SOItem item, int amount)
    {
        if (item.GetType() == typeof(SOUsableItem))
        {
            _usableInventorySO.RemoveItems(item, amount);
        }
        else if (item.GetType() == typeof(SOEquipmentItem))
        {
            _equipmentInventorySO.RemoveItems(item, amount);
        }
        else if (item.GetType() == typeof(SOCraftingItem))
        {
            _craftingInventorySO.RemoveItems(item, amount);
        }
    }
}