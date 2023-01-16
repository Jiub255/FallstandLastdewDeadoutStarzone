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

    private BuildingItemSO _buildingItemSO;

    public void SetupSlot(BuildingItemSO newBuildingItemSO)
    {
        _buildingItemSO = newBuildingItemSO;
        _icon.sprite = _buildingItemSO.Icon;
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

    // Called by clicking on building slot
	public void OnUseButton()
    {
        if (_buildingItemSO != null)
        {
            _buildingItemSO.Use();
        }
    }
}