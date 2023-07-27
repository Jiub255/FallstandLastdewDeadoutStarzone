using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _amountText;

    private SOItem _itemSO;

    private Image Icon { get { return _icon; } }
    private TextMeshProUGUI AmountText { get { return _amountText; } }
    private SOItem ItemSO { get { return _itemSO; } set { _itemSO = value; } }

    public void SetupSlot(ItemAmount itemAmount)
    {
        ItemSO = itemAmount.ItemSO;
        Icon.sprite = ItemSO.Icon; 
        AmountText.text = (itemAmount.Amount == 1) ? "" : itemAmount.Amount.ToString();
    }

    // Called by clicking on inventory slot
	public void OnClickInventoryItem()
    {
        if (ItemSO != null)
        {
            ItemSO.OnClickInventory();
        }
    }
}