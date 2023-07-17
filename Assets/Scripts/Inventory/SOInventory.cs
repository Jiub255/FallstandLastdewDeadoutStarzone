using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory SO", menuName = "Items/SOInventory")]
public class SOInventory/*<T>*/ : ScriptableObject, IResettable/*, IGenericSOInventory where T : SOItem*/
{
    public event Action OnInventoryChanged;
    // Serialized for now just to see in inspector. Can get rid of the protected field entirely when done testing. 
    [SerializeField]
    protected List<ItemAmount> _itemAmounts = new List<ItemAmount>();

    public List<ItemAmount/*<T>*/> ItemAmounts { get { return _itemAmounts; } protected set { _itemAmounts = value; } }

    public ItemAmount/*<T>*/ Contains(SOItem/*T*/ item, int amount = 1)
    {
        foreach (ItemAmount/*<T>*/ itemAmount in ItemAmounts)
        {
            if (itemAmount.ItemSO == item && itemAmount.Amount >= amount)
            {
                return itemAmount;
            }
        }

        return null;
    }

    public void AddItems(SOItem/*T*/ item, int amount)
    {
        ItemAmount/*<T>*/ listItemAmount = Contains(item);
        // Increase amount if item already in list. 
        if (listItemAmount != null)
        {
            listItemAmount.Amount += amount;
        }
        // Add new itemAmount to list if not. 
        else
        {
            ItemAmount/*<T>*/ newItemAmount = new(item, amount);
            ItemAmounts.Add(newItemAmount);
        }

        // Heard by UIInventory, calls SetupSlots with the newly updated inventory SO. 
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItems(SOItem/*T*/ item, int amount)
    {
        ItemAmount/*<T>*/ listItemAmount = Contains(item);
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
//        ItemAmounts.Clear();
    }
}