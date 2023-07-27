using System;
using System.Collections.Generic;

/// <summary>
/// Have BuildingManager created in here? So it can get required items? Kinda functions like CraftingManager so it makes sense. 
/// </summary>
public class InventoryManager
{
    /// <summary>
    /// TODO - Have GameManager hear this, and recalculate possible recipes based off of Inventory and Stats. Pass CraftingInventory? <br/>
    /// Currently heard by UIRecipes, who calculates possible recipes and keeps the list. Going to put list on CurrentTeamSO and have GameManager calculate it. 
    /// </summary>
    public static event Action OnInventoryChanged;

    private SOInventoryData InventoryDataSO { get; set; }
    private CraftingManager CraftingManager { get; set; }

    public InventoryManager(SOInventoryData inventoryDataSO)
    {
        InventoryDataSO = inventoryDataSO;
        
        CraftingManager = new(inventoryDataSO.CraftingInventorySO);

        SOItem.OnSelectItem += (itemSO) => AddItems(CraftingManager.HandleCrafting(itemSO));
        SOItem.OnAddItem += (item) => AddItems(item);
        EquipmentManager.OnUnequip += (equipmentItem) => AddItems(equipmentItem);
        PCLootState.OnLootItems += (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);
        EquipmentManager.OnEquip += (equipmentItem) => RemoveItems(equipmentItem);
        SOItem.OnRemoveItem += (item) => RemoveItems(item);
    }

    public void OnDisable()
    {
        SOItem.OnSelectItem -= (itemSO) => AddItems(CraftingManager.HandleCrafting(itemSO));
        SOItem.OnAddItem -= (item) => AddItems(item);
        EquipmentManager.OnUnequip -= (equipmentItem) => AddItems(equipmentItem);
        PCLootState.OnLootItems -= (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);
        EquipmentManager.OnEquip -= (equipmentItem) => RemoveItems(equipmentItem);
        SOItem.OnRemoveItem -= (item) => RemoveItems(item);
    }

    // TODO - Do this in CraftingManager instead? 
    public List<SORecipe> GetHaveEnoughItemsRecipes(List<SORecipe> metRequirementsRecipes)
    {
        List<SORecipe> haveEnoughItemsRecipes = new();

        foreach (SORecipe recipe in metRequirementsRecipes)
        {
            foreach (RecipeCost recipeCost in recipe.RecipeCosts)
            {
                // If you don't have enough items to craft the recipe,  
                if (InventoryDataSO.CraftingInventorySO.Contains(recipeCost.CraftingItemSO, recipeCost.Amount) == null)
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

    public virtual void AddItems(SOItem item, int amount = 1)
    {
        if (item != null)
        {
            if (item.GetType() == typeof(SOUsableItem))
            {
                InventoryDataSO.UsableInventorySO.AddItems(item, amount);
            }
            else if (item.GetType() == typeof(SOEquipmentItem))
            {
                InventoryDataSO.EquipmentInventorySO.AddItems(item, amount);
            }
            else if (item.GetType() == typeof(SOCraftingItem))
            {
                InventoryDataSO.CraftingInventorySO.AddItems(item, amount);
            }
        }

        // Heard by UIRecipes, updates haveEnoughItemsRecipes list. 
        OnInventoryChanged?.Invoke();
    }

    public virtual void RemoveItems(SOItem item, int amount = 1)
    {
        if (item.GetType() == typeof(SOUsableItem))
        {
            InventoryDataSO.UsableInventorySO.RemoveItems(item, amount);
        }
        else if (item.GetType() == typeof(SOEquipmentItem))
        {
            InventoryDataSO.EquipmentInventorySO.RemoveItems(item, amount);
        }
        else if (item.GetType() == typeof(SOCraftingItem))
        {
            InventoryDataSO.CraftingInventorySO.RemoveItems(item, amount);
        }

        // Heard by UIRecipes, updates haveEnoughItemsRecipes list. 
        OnInventoryChanged?.Invoke();
    }
}