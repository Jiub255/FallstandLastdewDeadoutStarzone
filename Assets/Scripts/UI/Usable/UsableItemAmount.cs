[System.Serializable]
public class UsableItemAmount
{
	public UsableItemSO UsableItemSO;
	public int Amount = 1;

	public UsableItemAmount(UsableItemSO usableItemSO, int amount = 1)
    {
		UsableItemSO = usableItemSO;
		Amount = amount;
    }
}