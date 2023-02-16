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

    private GameObject _pcInstance;

    public void SetupSlot(GameObject pcInstance)
    {
        _pcInstance = pcInstance;
        _icon.sprite = pcInstance.GetComponentInChildren<PCInfo>().Icon;
        _nameText.text = pcInstance.name;
        _icon.enabled = true;
        _useButton.interactable = true;
    }

    public void ClearSlot()
    {
        _pcInstance = null;
        _icon.sprite = null;
        _nameText.text = "";
        _icon.enabled = false;
        _useButton.interactable = false;
    }

    // Called by clicking on PC slot
    public void OnUseButton()
    {
        if (_pcInstance != null)
        {
            _pcInstance.GetComponentInChildren<PCInfo>().Use();
        }
    }
}