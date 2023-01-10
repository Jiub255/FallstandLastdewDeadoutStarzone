using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Button useButton;
    [SerializeField]
    private TextMeshProUGUI amountText;

    private ItemAmount itemAmount;

    public void SetupSlot(ItemAmount newItemAmount)
    {
        itemAmount = newItemAmount;

        icon.sprite = newItemAmount.inventoryItemSO.itemIcon;
        icon.enabled = true;
        useButton.interactable = true;

        if (newItemAmount.amount == 1)
        {
            amountText.text = "";
        }
        else
        {
            amountText.text = newItemAmount.amount.ToString();
        }
    }

    public void ClearSlot()
    {
        itemAmount = null;

        icon.sprite = null;
        icon.enabled = false;
        useButton.interactable = false;
        amountText.text = "";
    }

    // Called by clicking on inventory slot
	public void OnUseButton()
    {
        if (itemAmount != null)
        {
            // TODO: Set Use() up in Item and its child classes
            itemAmount.inventoryItemSO.Use();
        }
    }
}