using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Items/InventorySO")]
public class InventorySO : ScriptableObject
{
    public List<ItemAmount> ItemAmounts = new List<ItemAmount>();

    public ItemAmount GetItemAmount(InventoryItemSO inventoryItemSO)
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

// TODO: Put amount in ItemSO class, get rid of this ItemAmount class.
// Only needed it for another game because multiple characters used the same items in their inventories.
// Not an issue in this game.
[System.Serializable]
public class ItemAmount
{
    public InventoryItemSO InventoryItemSO;
    public int Amount;

    public ItemAmount(InventoryItemSO inventoryItemSO, int amount)
    {
        this.InventoryItemSO = inventoryItemSO;
        this.Amount = amount;
    }
}