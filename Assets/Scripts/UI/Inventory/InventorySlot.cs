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

    public void SetupSlot(ItemAmount itemAmount)
    {
        _itemSO = itemAmount.ItemSO;
        _icon.sprite = _itemSO.Icon; 
        _amountText.text = (itemAmount.Amount == 1) ? "" : itemAmount.Amount.ToString();
    }

    // Called by clicking on inventory slot
	public void OnClickInventoryItem()
    {
        if (_itemSO != null)
        {
            _itemSO.OnClickInventory();
        }
    }
}