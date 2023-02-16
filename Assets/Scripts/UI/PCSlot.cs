using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PCSlot : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Button _useButton;
    [SerializeField]
    private TextMeshProUGUI _nameText;

    // TODO: Replace these SO's with PC prefabs. 
    private PCItemSO _PCItemSO;

    public void SetupSlot(PCItemSO newPCItemSO)
    {
        _PCItemSO = newPCItemSO;
        _icon.sprite = newPCItemSO.Icon;
        _icon.enabled = true;
        _useButton.interactable = true;
        // Could just use newPCItemSO.PCInstance.name instead?
        _nameText.text = newPCItemSO.Name.ToString();
    }

    public void ClearSlot()
    {
        _PCItemSO = null;
        _icon.sprite = null;
        _icon.enabled = false;
        _useButton.interactable = false;
        _nameText.text = "";
    }

    // Called by clicking on PC slot
    public void OnUseButton()
    {
        if (_PCItemSO != null)
        {
            _PCItemSO.Use();
        }
    }
}