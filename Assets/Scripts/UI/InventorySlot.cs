using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Button _useButton;
    [SerializeField]
    private TextMeshProUGUI _amountText;

    private ItemAmount _itemAmount;

    public void SetupSlot(ItemAmount newItemAmount)
    {
        _itemAmount = newItemAmount;

        _icon.sprite = newItemAmount.InventoryItemSO.ItemIcon;
        _icon.enabled = true;
        _useButton.interactable = true;

        if (newItemAmount.Amount == 1)
        {
            _amountText.text = "";
        }
        else
        {
            _amountText.text = newItemAmount.Amount.ToString();
        }
    }

    public void ClearSlot()
    {
        _itemAmount = null;

        _icon.sprite = null;
        _icon.enabled = false;
        _useButton.interactable = false;
        _amountText.text = "";
    }

    // Called by clicking on inventory slot
	public void OnUseButton()
    {
        if (_itemAmount != null)
        {
            // TODO: Set Use() up in Item and its child classes
            _itemAmount.InventoryItemSO.Use();
        }
    }
}