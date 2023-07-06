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

    public void SetupSlot(SOItem itemSO, int amount = 1/*ItemAmount<SOItem> newItemAmount*/)
    {
        _itemSO = itemSO;
        _icon.sprite = itemSO.Icon; 
        _amountText.text = (amount == 1) ? "" : amount.ToString();
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