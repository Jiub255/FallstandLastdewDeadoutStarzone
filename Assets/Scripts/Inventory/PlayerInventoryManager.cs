// Extending InventoryManager to listen for add/remove item events, 
// so that those events don't affect every inventory in the scene, only the player's. 
// Also to handle crafting. 
using System;

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

        PlayerLootState.OnLootItems += (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);

        UICrafting.OnGetHaveEnoughItemsRecipes += _craftingManager.GetHaveEnoughItemsRecipes;

        SOEquipment.OnUnequip += (equipmentItem) => AddItems(equipmentItem);
    }

    protected void OnDisable()
    {
        SOItem.OnSelectItem -= _craftingManager.HandleCrafting;
        _craftingManager.OnCraftItem -= (itemSO) => AddItems(itemSO, 1);

        SOItem.OnAddItem -= (item) => AddItems(item, 1);
        SOItem.OnRemoveItem -= (item) => RemoveItems(item, 1);
   
        PlayerLootState.OnLootItems -= (itemAmount) => AddItems(itemAmount.ItemSO, itemAmount.Amount);

        UICrafting.OnGetHaveEnoughItemsRecipes -= _craftingManager.GetHaveEnoughItemsRecipes;

        SOEquipment.OnUnequip -= (equipmentItem) => AddItems(equipmentItem);
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
}