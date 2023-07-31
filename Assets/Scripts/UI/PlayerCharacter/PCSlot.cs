using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PCSlot : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private Image _painFillbar;
    [SerializeField]
    private Image _injuryFillbar;

    private SOPCData _pcSO;

    public void UpdatePainBar(int pain)
    {
        _painFillbar.fillAmount = (float)pain / 100f;
    }

    public void UpdateInjuryBar(int injury)
    {
        _injuryFillbar.fillAmount = (float)injury / 100f;
    }

    public void SetupSlot(SOPCData pcSO)
    {
        _pcSO = pcSO;
        _icon.sprite = pcSO.Icon;
        _nameText.text = pcSO.name;

        // Set the fill bars here initially, but they get changed from the pain and injury scripts.
        UpdatePainBar(pcSO.Pain);
        UpdateInjuryBar(pcSO.Injury);

        // Set the pain and injury script's reference to this script here, so it can update the UI when it changes value. 
        pcSO.PCController.PainInjuryManager.Slot = this;
    }

    // Called by clicking on PC slot
    public void OnUseButton()
    {
        if (_pcSO != null)
        {
            _pcSO.SelectPC();
        }
    }
}