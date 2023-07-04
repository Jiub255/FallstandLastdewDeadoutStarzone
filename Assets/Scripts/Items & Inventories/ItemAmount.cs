[System.Serializable]
public class ItemAmount<T> where T : SOItem
{
    public T ItemSO;
    public int Amount = 1;

    public ItemAmount(T itemSO, int amount)
    {
        ItemSO = itemSO;
        Amount = amount;
    }
}