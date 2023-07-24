using System;
using System.Collections.Generic;

// Extending InventoryManager to listen for add/remove item events, 
// so that those events don't affect every inventory in the scene, only the player's. 
// Also to handle crafting. 
// Is this necessary? Will there be any other inventories? Might be okay to combine this into InventoryManager, and just 
// put it on the singleton or something. 
public class PlayerInventoryManager : InventoryManager
{
    public static event Action OnPlayerInventoryChanged;

    protected CraftingManager _craftingManager;

    protected void OnEnable()
    {
        _craftingManager = new(_craftingInventorySO);

        SOItem.OnSelectItem += (itemSO) => AddItems(_craftingManager.HandleCrafting(itemSO));
        SOItem.OnAddItem += (item) => AddItems(item);
        EquipmentManager.OnUnequip += (equipmentItem) => AddItems(equipmentItem);
        PCLootState.OnLootItems += (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);
        EquipmentManager.OnEquip += (equipmentItem) => RemoveItems(equipmentItem);
        SOItem.OnRemoveItem += (item) => RemoveItems(item);
        UIRecipes.OnGetHaveEnoughItemsRecipes += GetHaveEnoughItemsRecipes;
    }

    protected void OnDisable()
    {
        SOItem.OnSelectItem -= (itemSO) => AddItems(_craftingManager.HandleCrafting(itemSO));
        SOItem.OnAddItem -= (item) => AddItems(item);
        EquipmentManager.OnUnequip -= (equipmentItem) => AddItems(equipmentItem);
        PCLootState.OnLootItems -= (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);
        EquipmentManager.OnEquip -= (equipmentItem) => RemoveItems(equipmentItem);
        SOItem.OnRemoveItem -= (item) => RemoveItems(item);
        UIRecipes.OnGetHaveEnoughItemsRecipes -= GetHaveEnoughItemsRecipes;
    }

    public override void AddItems(SOItem item, int amount = 1)
    {
        if (item != null)
        {
            base.AddItems(item, amount);

            // Heard by UIRecipes, updates haveEnoughItemsRecipes list. 
            OnPlayerInventoryChanged?.Invoke();
        }
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