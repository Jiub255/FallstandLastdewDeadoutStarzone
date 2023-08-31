using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO - Keep just the data here, and make an InventoryController class to do the Add/Remove/Contains? 
/// </summary>
[CreateAssetMenu(menuName = "Inventory/SOInventory", fileName = "New Inventory SO")]
public class SOInventory : ScriptableObject, IResettable/*, IGenericSOInventory where T : SOItem*/
{
    public event Action OnInventoryChanged;

    // Serialized for now just to see in inspector. 
    [field: SerializeField]
    public List<ItemAmount> ItemAmounts { get; private set; } = new();

    /// <summary>
    /// Called by InventoryController. 
    /// </summary>
    public void InventoryChanged()
    {
        OnInventoryChanged?.Invoke();
    }

/*    /// <summary>
    /// Returns the reference to the item amount in inventory if you have enough. Doesn't return the amount you put in necessarily,
    /// just how much you have in inventory. Returns null if you don't have enough or don't have an ItemAmount with the same SOItem at all. 
    /// </summary>
    public ItemAmount Contains(SOItem item, int amount = 1)
    {
        foreach (ItemAmount itemAmount in ItemAmounts)
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
            ItemAmounts.Add(newItemAmount);
        }

        // Heard by UIInventory, calls SetupSlots with the newly updated inventory SO. 
        OnInventoryChanged?.Invoke();
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
                ItemAmounts.Remove(listItemAmount);
            }
            // Else, if there's less than [amount] in inventory, warn and don't do anything. 
            else
            {
                Debug.LogWarning($"Only {listItemAmount.Amount} {listItemAmount.ItemSO.name}s left, can't remove {amount}");
            }

            OnInventoryChanged?.Invoke();

            return;
        }
        else
        {
            Debug.LogWarning("Item to be removed not in inventory");
        }
    }*/

    // FOR TESTING
    // Clear inventory when exiting play mode. 
    public void ResetOnExitPlayMode()
    {
//        ItemAmounts.Clear();
    }
}