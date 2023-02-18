using UnityEngine;

public class CraftingUI : UIRefresher
{
    [SerializeField]
    private CraftingInventorySO _craftingInventorySO;

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

        foreach (CraftingItemAmount craftingItemAmount in _craftingInventorySO.CraftingItemAmounts)
        {
            GameObject slotInstance = Instantiate(SlotPrefab, SlotParent);

            slotInstance.transform.GetComponent<CraftingSlot>().SetupSlot(craftingItemAmount);
        }
    }
}