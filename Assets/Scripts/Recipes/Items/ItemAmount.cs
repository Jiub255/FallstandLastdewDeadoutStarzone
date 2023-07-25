using UnityEngine;

/// <summary>
/// Using this class instead of an (SOItem, int) tuple so it can be shown in Unity inspector. 
/// </summary>
[System.Serializable]
public class ItemAmount
{
	[SerializeField]
	private SOItem _itemSO;
	[SerializeField]
	private int _amount;

	public SOItem ItemSO { get { return _itemSO; } }
	public int Amount { get { return _amount; } set { _amount = value; } }

	public ItemAmount(SOItem itemSO, int amount)
    {
		_itemSO = itemSO;
		_amount = amount;
    }
}