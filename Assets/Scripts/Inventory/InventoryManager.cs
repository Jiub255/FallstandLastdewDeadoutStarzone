using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private SOInventoryData InventoryDataSO { get; }
    private InventoryController CraftingInventoryController { get; }
    private InventoryController EquipmentInventoryController { get; }
    private InventoryController UsableItemsInventoryController { get; }
    private InventoryController ToolInventoryController { get; }
    private CraftingHandler CraftingHandler { get; /*set; */}

    public InventoryManager(SOInventoryData inventoryDataSO)
    {
        InventoryDataSO = inventoryDataSO;

        // Must be called before instantiating CraftingHandler. 
        CraftingInventoryController = new InventoryController(InventoryDataSO.CraftingInventorySO);
        EquipmentInventoryController = new InventoryController(InventoryDataSO.EquipmentInventorySO);
        UsableItemsInventoryController = new InventoryController(InventoryDataSO.UsableItemsInventorySO);
        ToolInventoryController = new InventoryController(InventoryDataSO.ToolInventorySO);

        // Must be called after instantiating CraftingInventoryController. 
        CraftingHandler = new(CraftingInventoryController);

        SOItem.OnSelectItem += (itemSO) => AddItems(CraftingHandler.HandleCrafting(itemSO));
        SOItem.OnAddItem += (item) => AddItems(item);
        EquipmentManager.OnUnequip += (equipmentItem) => AddItems(equipmentItem);
        PCLootState.OnLootItems += (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);
        EquipmentManager.OnEquip += (equipmentItem) => RemoveItems(equipmentItem);
        SOItem.OnRemoveItem += (item) => RemoveItems(item);
    }

    public void OnDisable()
    {
        SOItem.OnSelectItem -= (itemSO) => AddItems(CraftingHandler.HandleCrafting(itemSO));
        SOItem.OnAddItem -= (item) => AddItems(item);
        EquipmentManager.OnUnequip -= (equipmentItem) => AddItems(equipmentItem);
        PCLootState.OnLootItems -= (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);
        EquipmentManager.OnEquip -= (equipmentItem) => RemoveItems(equipmentItem);
        SOItem.OnRemoveItem -= (item) => RemoveItems(item);
    }

    // TODO - Do this in CraftingManager instead? 
    /// <summary>
    /// Filters out all of the SORecipes that you either don't have enough crafting materials to build, <br/>
    /// or you don't have the required tools for. 
    /// </summary>
    public List<T> GetHaveEnoughItemsRecipes<T>(List<T> metRequirementsRecipes) where T : SORecipe
    {
        Debug.Log($"Pre items filtered list count: {metRequirementsRecipes.Count}");

        // Does this fancy LINQ work? 
        // Returns the SORecipes that you have enough items to build, and have the required tools for. 
        List<T> filteredList = metRequirementsRecipes
            .Where(recipeSO => recipeSO.RecipeCosts
            .Where(recipeCost => CraftingInventoryController
            .Contains(recipeCost.CraftingItemSO, recipeCost.Amount) == null)
            .ToList().Count == 0 && 
            recipeSO.RequiredTools
            .Where(toolSO => ToolInventoryController
            .Contains(toolSO) == null)
            .ToList().Count == 0)
            .ToList();

        Debug.Log($"Post items filtered list count: {filteredList.Count}");

        return filteredList;

/*        List<T> haveEnoughItemsRecipes = new();

        foreach (T recipe in metRequirementsRecipes)
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

        return haveEnoughItemsRecipes;*/
    }

    public virtual void AddItems(SOItem item, int amount = 1)
    {
        if (item != null)
        {
            if (item.GetType() == typeof(SOUsableItem))
            {
                UsableItemsInventoryController.AddItems(item, amount);
            }
            else if (item.GetType() == typeof(SOEquipmentItem))
            {
                EquipmentInventoryController.AddItems(item, amount);
            }
            else if (item.GetType() == typeof(SOCraftingItem))
            {
                CraftingInventoryController.AddItems(item, amount);
            }
            else if (item.GetType() == typeof(SOTool))
            {
                ToolInventoryController.AddItems(item, amount);
            }
        }

        // Heard by UIRecipes, updates haveEnoughItemsRecipes list. 
        OnInventoryChanged?.Invoke();
    }

    public virtual void RemoveItems(SOItem item, int amount = 1)
    {
        if (item.GetType() == typeof(SOUsableItem))
        {
            UsableItemsInventoryController.RemoveItems(item, amount);
        }
        else if (item.GetType() == typeof(SOEquipmentItem))
        {
            EquipmentInventoryController.RemoveItems(item, amount);
        }
        else if (item.GetType() == typeof(SOCraftingItem))
        {
            CraftingInventoryController.RemoveItems(item, amount);
        }
        else if (item.GetType() == typeof(SOTool))
        {
            ToolInventoryController.RemoveItems(item, amount);
        }

        // Heard by UIRecipes, updates haveEnoughItemsRecipes list. 
        OnInventoryChanged?.Invoke();
    }
}