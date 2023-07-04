using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    protected SOUsableInventory _usableInventorySO;
    [SerializeField]
    protected SOEquipmentInventory _equipmentInventorySO;
    [SerializeField]
    protected SOCraftingInventory _craftingInventorySO;

    // Is there a cleaner way to do this? 
    public void AddItems(SOItem item, int amount)
    {
        if (item.GetType() == typeof(SOUsableItem))
        {
            _usableInventorySO.AddItems((SOUsableItem)item, amount);
        }
        else if (item.GetType() == typeof(SOEquipmentItem))
        {
            _equipmentInventorySO.AddItems((SOEquipmentItem)item, amount);
        }
        else if (item.GetType() == typeof(SOCraftingItem))
        {
            _craftingInventorySO.AddItems((SOCraftingItem)item, amount);
        }
    }

    public void RemoveItems(SOItem item, int amount)
    {
        if (item.GetType() == typeof(SOUsableItem))
        {
            _usableInventorySO.RemoveItems((SOUsableItem)item, amount);
        }
        else if (item.GetType() == typeof(SOEquipmentItem))
        {
            _equipmentInventorySO.RemoveItems((SOEquipmentItem)item, amount);
        }
        else if (item.GetType() == typeof(SOCraftingItem))
        {
            _craftingInventorySO.RemoveItems((SOCraftingItem)item, amount);
        }
    }
}