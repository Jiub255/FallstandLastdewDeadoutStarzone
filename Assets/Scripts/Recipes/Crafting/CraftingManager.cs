using UnityEngine;

public class CraftingManager
{
    protected SOInventory _craftingInventorySO;

    public CraftingManager(SOInventory craftingInventorySO)
    {
        _craftingInventorySO = craftingInventorySO;
    }

    public SOItem HandleCrafting(SOItem itemSO)
    {
        foreach (RecipeCost recipeCost in itemSO.RecipeCosts)
        {
            if (_craftingInventorySO.Contains(recipeCost.CraftingItemSO, recipeCost.Amount) == null)
            {
                // TODO - Make a UI text that says how much more of each material you need to craft the item? 
                Debug.Log($"Don't have enough crafting materials to craft {itemSO.name}");
                return null;
            }
        }

        // Can only reach this point if you have at least recipeCost.Amount of each recipeCost.CraftingItemSO in your inventory.
        return CraftItem(itemSO);
    }

    private SOItem CraftItem(SOItem itemSO)
    {
        foreach (RecipeCost recipeCost in itemSO.RecipeCosts)
        {
            _craftingInventorySO.RemoveItems(recipeCost.CraftingItemSO, recipeCost.Amount);
        }

        return itemSO;
    }
}