[System.Serializable]
public class OLDItemAmount
{
    public ItemSO ItemSO;
    public int Amount = 1;

    public OLDItemAmount(ItemSO itemSO, int amount = 1)
    {
        ItemSO = itemSO;
        Amount = amount;
    }
}