using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Button _useButton;
    [SerializeField]
    private TextMeshProUGUI _amountText;

    private EquipmentItemAmount _equipmentItemAmount;

    public void SetupSlot(EquipmentItemAmount equipmentItemAmount)
    {
        _equipmentItemAmount = equipmentItemAmount;

        _icon.sprite = equipmentItemAmount.EquipmentItemSO.Icon;
        _icon.enabled = true;
        _useButton.interactable = true;

        if (equipmentItemAmount.Amount == 1)
        {
            _amountText.text = "";
        }
        else
        {
            _amountText.text = equipmentItemAmount.Amount.ToString();
        }
    }

    public void ClearSlot()
    {
        _equipmentItemAmount = null;

        _icon.sprite = null;
        _icon.enabled = false;
        _useButton.interactable = false;
        _amountText.text = "";
    }

    // Called by clicking on inventory slot
    public void OnUseButton()
    {
        if (_equipmentItemAmount != null)
        {
            // TODO: Set Use() up in Item and its child classes
            _equipmentItemAmount.EquipmentItemSO.Use();
        }
    }
}