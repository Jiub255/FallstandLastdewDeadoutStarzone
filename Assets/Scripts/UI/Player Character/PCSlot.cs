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
    [SerializeField]
    private Image _painFillbar;
    [SerializeField]
    private Image _injuryFillbar;

    private GameObject _pcInstance;

    public void UpdatePainBar(int pain)
    {
        _painFillbar.fillAmount = (float)pain / 100f;
    }

    public void UpdateInjuryBar(int injury)
    {
        _injuryFillbar.fillAmount = (float)injury / 100f;
    }

    public void SetupSlot(GameObject pcInstance)
    {
        _pcInstance = pcInstance;
        _icon.sprite = pcInstance.GetComponentInChildren<PCInfo>().Icon;
        _nameText.text = pcInstance.name;
        _icon.enabled = true;
        _useButton.interactable = true;

        // Set the fill bars here initially, but they get changed from the pain and injury scripts.
        UpdateInjuryBar(pcInstance.GetComponentInChildren<PlayerInjury>().Injury);
        UpdatePainBar(pcInstance.GetComponentInChildren<PlayerPain>().EffectivePain);
        // Set the pain and injury scripts' references to this script here, so they can update the UI when they change value. 
        pcInstance.GetComponentInChildren<PlayerInjury>().Slot = this;
        pcInstance.GetComponentInChildren<PlayerPain>().Slot = this;
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