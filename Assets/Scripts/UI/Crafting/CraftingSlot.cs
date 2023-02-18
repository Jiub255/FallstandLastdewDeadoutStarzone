using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Button _useButton;
    [SerializeField]
    private TextMeshProUGUI _amountText;

    private CraftingItemAmount _craftingItemAmount;

    public void SetupSlot(CraftingItemAmount craftingItemAmount)
    {
        _craftingItemAmount = craftingItemAmount;

        _icon.sprite = craftingItemAmount.CraftingItemSO.Icon;
        _icon.enabled = true;
        _useButton.interactable = true;

        if (craftingItemAmount.Amount == 1)
        {
            _amountText.text = "";
        }
        else
        {
            _amountText.text = craftingItemAmount.Amount.ToString();
        }
    }

    public void ClearSlot()
    {
        _craftingItemAmount = null;

        _icon.sprite = null;
        _icon.enabled = false;
        _useButton.interactable = false;
        _amountText.text = "";
    }

    // Called by clicking on inventory slot
    public void OnUseButton()
    {
        if (_craftingItemAmount != null)
        {
            // TODO: Set Use() up in Item and its child classes
            _craftingItemAmount.CraftingItemSO.Use();
        }
    }
}