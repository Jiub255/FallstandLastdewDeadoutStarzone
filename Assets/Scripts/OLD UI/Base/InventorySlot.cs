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
    private int _amount;
//    private ItemAmount<SOItem> _itemAmount;

    public void SetupSlot(SOItem itemSO, int amount/*ItemAmount<SOItem> newItemAmount*/)
    {
        _itemSO = itemSO;
        _amount = amount;
//        _itemAmount = newItemAmount;

        _icon.sprite = itemSO.Icon; 
        _amountText.text = (amount == 1) ? "" : amount.ToString();
    }

    // Called by clicking on inventory slot
	public void OnUseButton()
    {
        if (_itemSO != null)
        {
            _itemSO.OnClickFromInventory();
        }
    }
}