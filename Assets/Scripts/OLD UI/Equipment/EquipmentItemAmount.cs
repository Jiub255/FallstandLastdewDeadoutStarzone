[System.Serializable]
public class EquipmentItemAmount
{
	public EquipmentItemSO EquipmentItemSO;
	public int Amount = 1;

	public EquipmentItemAmount(EquipmentItemSO equipmentItemSO, int amount = 1)
    {
		EquipmentItemSO = equipmentItemSO;
		Amount = amount;
    }
}