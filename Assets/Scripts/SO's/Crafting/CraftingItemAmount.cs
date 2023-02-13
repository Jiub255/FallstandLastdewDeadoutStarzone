[System.Serializable]
public class CraftingItemAmount
{
	public CraftingItemSO CraftingItemSO;
	public int Amount; 

	public CraftingItemAmount(CraftingItemSO craftingItemSO, int amount)
    {
		CraftingItemSO = craftingItemSO;
		Amount = amount;
    }
}