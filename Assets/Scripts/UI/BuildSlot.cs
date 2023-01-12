using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildSlot : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Button _useButton;
    [SerializeField]
    private TextMeshProUGUI _costText;

    // Need to have a separate list of Build Items, separate inv/invSO too I think.
    // Makes sense, they're very different things, inv and buildinv.
    private BuildingItemSO _buildingItemSO;

    public void SetupSlot(BuildingItemSO newBuildingItemSO)
    {
        _buildingItemSO = newBuildingItemSO;
        _icon.sprite = _buildingItemSO.ItemIcon;
        _icon.enabled = true;
        _useButton.interactable = true;
        _costText.text = newBuildingItemSO.Cost.ToString();
    }

    public void ClearSlot()
    {
        _buildingItemSO = null;
        _icon.sprite = null;
        _icon.enabled = false;
        _useButton.interactable = false;
        _costText.text = "";
    }

    // Called by clicking on inventory slot
	public void OnUseButton()
    {
        if (_buildingItemSO != null)
        {
            // TODO: Set Use() up in Item and its child classes
            _buildingItemSO.Use();
        }
    }
}