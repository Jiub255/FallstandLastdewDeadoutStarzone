using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOInventory<T> : ScriptableObject, IResettable, IGenericSOInventory where T : SOItem
{
    public event Action OnInventoryChanged;

    public List<ItemAmount<T>> ItemAmounts { get; protected set; }

    public ItemAmount<T> Contains(T item)
    {
        foreach (ItemAmount<T> itemAmount in ItemAmounts)
        {
            if (itemAmount.ItemSO == item)
            {
                return itemAmount;
            }
        }

        return null;
    }

    public void AddItems(T item, int amount)
    {
        ItemAmount<T> listItemAmount = Contains(item);
        // Increase amount if item already in list. 
        if (listItemAmount != null)
        {
            listItemAmount.Amount += amount;
        }
        // Add new itemAmount to list if not. 
        else
        {
            ItemAmount<T> newItemAmount = new(item, amount);
            ItemAmounts.Add(newItemAmount);
        }

        // Heard by UIInventory, calls SetupSlots with the newly updated inventory SO. 
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItems(T item, int amount)
    {
        ItemAmount<T> listItemAmount = Contains(item);
        if (listItemAmount != null)
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
    }

    // FOR TESTING
    // Clear inventory when exiting play mode. 
    public void ResetOnExitPlayMode()
    {
        ItemAmounts.Clear();
    }
}