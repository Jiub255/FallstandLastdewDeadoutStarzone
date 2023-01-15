[System.Serializable]
public class ItemAmount
{
    public InventoryItemSO InventoryItemSO;
    public int Amount = 1;

    public ItemAmount(InventoryItemSO inventoryItemSO, int amount)
    {
        this.InventoryItemSO = inventoryItemSO;
        this.Amount = amount;
    }
}