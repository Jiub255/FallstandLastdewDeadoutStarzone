using System;
using System.Collections.Generic;
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

    public List<SORecipe> GetHaveEnoughItemsRecipes(List<SORecipe> metRequirementsRecipes)
    {
        List<SORecipe> haveEnoughItemsRecipes = new();

        foreach (SORecipe recipe in metRequirementsRecipes)
        {
            foreach (RecipeCost recipeCost in recipe.RecipeCosts)
            {
                // If you don't have enough items to craft the recipe,  
                if (_craftingInventorySO.Contains(recipeCost.CraftingItemSO, recipeCost.Amount) == null)
                {
                    // Then go to next recipe. 
                    break;
                }
            }

            // Can only reach this point if you have at least recipeCost.Amount of each recipeCost.CraftingItemSO in your inventory.
            haveEnoughItemsRecipes.Add(recipe);
        }

        return haveEnoughItemsRecipes;
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
        // TODO - Maybe have a show all toggle, and otherwise it only shows recipes you have the items for? 
        OnCraftItem?.Invoke(itemSO);
    }
}