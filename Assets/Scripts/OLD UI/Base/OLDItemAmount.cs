[System.Serializable]
public class OLDItemAmount
{
    public ItemSO ItemSO;
    public int Amount;

    public OLDItemAmount(ItemSO itemSO, int amount = 1)
    {
        ItemSO = itemSO;
        Amount = amount;
    }
}