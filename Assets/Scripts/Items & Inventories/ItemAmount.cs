[System.Serializable]
public class ItemAmount/*<T> where T : SOItem*/
{
    public /*T*/SOItem ItemSO;
    public int Amount = 1;

    public ItemAmount(/*T*/SOItem itemSO, int amount)
    {
        ItemSO = itemSO;
        Amount = amount;
    }
}