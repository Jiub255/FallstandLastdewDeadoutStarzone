using System;
using UnityEngine;

// TODO - Make this a plain c# class and instantiate it in InventoryManager. 
public class CraftingManager
{
    public event Action<SOItem> OnCraftItem;

    protected SOInventory _craftingInventorySO;

    public CraftingManager(SOInventory craftingInventorySO)
    {
        _craftingInventorySO = craftingInventorySO;
    }

    public void HandleCrafting(SOItem itemSO)
    {
        foreach (RecipeCost recipeCost in itemSO.RecipeCosts)
        {
            if (_craftingInventorySO.Contains(recipeCost.CraftingItemSO, recipeCost.Amount) == null)
            {
                // TODO - Make a UI text that says how much more of each material you need to craft the item. 
                Debug.Log($"Don't have enough crafting materials to craft {itemSO.name}");
                return;
            }
        }

        // Can only reach this point if you have at least recipeCost.Amount of each recipeCost.CraftingItemSO in your inventory.
        CraftItem(itemSO);
    }

    private void CraftItem(SOItem itemSO)
    {
        foreach (RecipeCost recipeCost in itemSO.RecipeCosts)
        {
            _craftingInventorySO.RemoveItems(recipeCost.CraftingItemSO, recipeCost.Amount);
        }

        // PlayerInventoryManager listens, adds item to inventory. 
        OnCraftItem?.Invoke(itemSO);
    }
}