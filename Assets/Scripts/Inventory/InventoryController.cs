using UnityEngine;

public class InventoryController
{
    private SOInventory InventorySO { get; }

    public InventoryController(SOInventory inventorySO)
    {
        InventorySO = inventorySO;
//        inventorySO.InventoryController = this;
    }

    /// <summary>
    /// Returns the reference to the item amount in inventory if you have enough. Doesn't return the amount you put in necessarily,
    /// just how much you have in inventory. Returns null if you don't have enough or don't have an ItemAmount with the same SOItem at all. 
    /// </summary>
    public ItemAmount Contains(SOItem item, int amount = 1)
    {
        foreach (ItemAmount itemAmount in InventorySO.ItemAmounts)
        {
            if (itemAmount.ItemSO == item && itemAmount.Amount >= amount)
            {
                return itemAmount;
            }
        }

        return null;
    }

    public void AddItems(SOItem item, int amount)
    {
        ItemAmount listItemAmount = Contains(item);
        // Increase amount if item already in list. 
        if (listItemAmount != null)
        {
            listItemAmount.Amount += amount;
        }
        // Add new itemAmount to list if not. 
        else
        {
            ItemAmount newItemAmount = new(item, amount);
            InventorySO.ItemAmounts.Add(newItemAmount);
        }

        // Heard by UIInventory, calls SetupSlots with the newly updated inventory SO. 
        InventorySO.InventoryChanged();
//        OnInventoryChanged?.Invoke();
    }

    public void RemoveItems(SOItem item, int amount)
    {
        ItemAmount listItemAmount = Contains(item);
        if (listItemAmount.ItemSO != null)
        {
            // If there's more than [amount] in inventory, decrease amount.
            if (listItemAmount.Amount > amount)
            {
                listItemAmount.Amount -= amount;
            }
            // If there's exactly [amount] in inventory, delete the ItemAmount entirely. 
            else if (listItemAmount.Amount == amount)
            {
                InventorySO.ItemAmounts.Remove(listItemAmount);
            }
            // Else, if there's less than [amount] in inventory, warn and don't do anything. 
            else
            {
                Debug.LogWarning($"Only {listItemAmount.Amount} {listItemAmount.ItemSO.name}s left, can't remove {amount}");
            }


            InventorySO.InventoryChanged();
//            OnInventoryChanged?.Invoke();

            return;
        }
        else
        {
            Debug.LogWarning("Item to be removed not in inventory");
        }
    }
}