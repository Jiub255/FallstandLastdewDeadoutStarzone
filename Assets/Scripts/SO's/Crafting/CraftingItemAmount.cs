[System.Serializable]
public class CraftingItemAmount
{
	public CraftingItemSO CraftingItemSO;
	public int Amount; 

	public CraftingItemAmount(CraftingItemSO craftingItemSO, int amount = 1)
    {
		CraftingItemSO = craftingItemSO;
		Amount = amount;
    }
}