// Only extending InventoryManager to listen for add/remove item events, 
// so that those events don't affect every inventory in the scene, only the player's. 
public class PlayerInventoryManager : InventoryManager
{
    private void OnEnable()
    {
        SOItem.OnAddItem += (item) => AddItems(item, 1);
        SOItem.OnRemoveItem += (item) => RemoveItems(item, 1);
    }

    private void OnDisable()
    {
        SOItem.OnAddItem -= (item) => AddItems(item, 1);
        SOItem.OnRemoveItem -= (item) => RemoveItems(item, 1);
    }
}