using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory SO", menuName = "Scriptable Object/Inventory/Inventory SO")]
public class InventorySO : ScriptableObject
{
    public List<ItemAmount> ItemAmounts = new List<ItemAmount>();

    public ItemAmount GetItemAmountFromItemSO(InventoryItemSO inventoryItemSO)
    {
        foreach (ItemAmount itemAmount in ItemAmounts)
        {
            if (itemAmount.InventoryItemSO == inventoryItemSO)
            {
                return itemAmount;
            }
        }

        return null;
    }
}