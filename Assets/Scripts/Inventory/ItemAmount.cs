[System.Serializable]
public class ItemAmount
{
    public ItemSO ItemSO;
    public int Amount = 1;

    public ItemAmount(ItemSO itemSO, int amount = 1)
    {
        ItemSO = itemSO;
        Amount = amount;
    }
}