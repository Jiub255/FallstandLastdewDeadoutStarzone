using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Button _useButton;
    [SerializeField]
    private TextMeshProUGUI _amountText;

    private UsableItemAmount _usableItemAmount;

    public void SetupSlot(UsableItemAmount usableItemAmount)
    {
        _usableItemAmount = usableItemAmount;

        _icon.sprite = usableItemAmount.UsableItemSO.Icon; 
        _icon.enabled = true;
        _useButton.interactable = true;

        if (usableItemAmount.Amount == 1)
        {
            _amountText.text = "";
        }
        else
        {
            _amountText.text = usableItemAmount.Amount.ToString();
        }
    }

    public void ClearSlot()
    {
        _usableItemAmount = null;

        _icon.sprite = null;
        _icon.enabled = false;
        _useButton.interactable = false;
        _amountText.text = "";
    }

    // Called by clicking on inventory slot
	public void OnUseButton()
    {
        if (_usableItemAmount != null)
        {
            // TODO: Set Use() up in Item and its child classes
            _usableItemAmount.UsableItemSO.Use();
        }
    }
}