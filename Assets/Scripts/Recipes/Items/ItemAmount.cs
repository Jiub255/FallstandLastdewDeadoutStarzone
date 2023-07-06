[System.Serializable]
public class ItemAmount/*<T> where T : SOItem*/
{
    public /*T*/SOItem ItemSO;
    public int Amount;

    public ItemAmount(/*T*/SOItem itemSO, int amount = 1)
    {
        ItemSO = itemSO;
        Amount = amount;
    }
}