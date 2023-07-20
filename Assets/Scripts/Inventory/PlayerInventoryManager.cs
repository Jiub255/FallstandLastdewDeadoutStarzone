// Extending InventoryManager to listen for add/remove item events, 
// so that those events don't affect every inventory in the scene, only the player's. 
// Also to handle crafting. 
using System;
using System.Collections.Generic;

public class PlayerInventoryManager : InventoryManager
{
    public static event Action OnPlayerInventoryChanged;

    protected CraftingManager _craftingManager;

    protected void OnEnable()
    {
        _craftingManager = new(_craftingInventorySO);

        SOItem.OnSelectItem += _craftingManager.HandleCrafting;
        _craftingManager.OnCraftItem += (itemSO) => AddItems(itemSO);

        SOItem.OnAddItem += (item) => AddItems(item);
        SOItem.OnRemoveItem += (item) => RemoveItems(item);

        PCLootState.OnLootItems += (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);

        UIRecipes.OnGetHaveEnoughItemsRecipes += GetHaveEnoughItemsRecipes;

        EquipmentManager.OnUnequip += (equipmentItem) => AddItems(equipmentItem);
    }

    protected void OnDisable()
    {
        SOItem.OnSelectItem -= _craftingManager.HandleCrafting;
        _craftingManager.OnCraftItem -= (itemSO) => AddItems(itemSO, 1);

        SOItem.OnAddItem -= (item) => AddItems(item, 1);
        SOItem.OnRemoveItem -= (item) => RemoveItems(item, 1);
   
        PCLootState.OnLootItems -= (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);

        UIRecipes.OnGetHaveEnoughItemsRecipes -= GetHaveEnoughItemsRecipes;

        EquipmentManager.OnUnequip -= (equipmentItem) => AddItems(equipmentItem);
    }

    public override void AddItems(SOItem item, int amount = 1)
    {
        base.AddItems(item, amount);

        // Heard by UIRecipes, updates haveEnoughItemsRecipes list. 
        OnPlayerInventoryChanged?.Invoke();
    }

    public override void RemoveItems(SOItem item, int amount = 1)
    {
        base.RemoveItems(item, amount);

        // Heard by UIRecipes, updates haveEnoughItemsRecipes list. 
        OnPlayerInventoryChanged?.Invoke();
    }

    private List<SORecipe> GetHaveEnoughItemsRecipes(List<SORecipe> metRequirementsRecipes)
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
}