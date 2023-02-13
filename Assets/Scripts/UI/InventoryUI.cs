using UnityEngine;

public class InventoryUI : UIRefresher
{
    // Make specific inventories inherit from InventorySO so this script can be used by all invs? 
    [SerializeField]
    private InventorySO _inventorySO;

    private void OnEnable()
    {
        UIManager.OnOpenedMenu += PopulateInventory;
    }

    private void OnDisable()
    {
        UIManager.OnOpenedMenu -= PopulateInventory;
    }

    public override void PopulateInventory()
    {
        base.PopulateInventory();

        foreach (ItemAmount itemAmount in _inventorySO.ItemAmounts)
        {
            GameObject slotInstance = Instantiate(SlotPrefab, SlotParent);
            
            slotInstance.transform.GetComponent<InventorySlot>().SetupSlot(itemAmount);
        }
    }
}