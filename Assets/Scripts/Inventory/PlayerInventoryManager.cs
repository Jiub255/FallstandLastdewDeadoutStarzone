// Extending InventoryManager to listen for add/remove item events, 
// so that those events don't affect every inventory in the scene, only the player's. 
// Also to handle crafting. 
public class PlayerInventoryManager : InventoryManager
{
    protected CraftingManager _craftingManager;

    protected void OnEnable()
    {
        _craftingManager = new(_craftingInventorySO);

        SOItem.OnSelectItem += _craftingManager.HandleCrafting;
        _craftingManager.OnCraftItem += (itemSO) => AddItems(itemSO, 1);

        SOItem.OnAddItem += (item) => AddItems(item, 1);
        SOItem.OnRemoveItem += (item) => RemoveItems(item, 1);
    }

    protected void OnDisable()
    {
        SOItem.OnSelectItem -= _craftingManager.HandleCrafting;
        _craftingManager.OnCraftItem -= (itemSO) => AddItems(itemSO, 1);

        SOItem.OnAddItem -= (item) => AddItems(item, 1);
        SOItem.OnRemoveItem -= (item) => RemoveItems(item, 1);
    }
}