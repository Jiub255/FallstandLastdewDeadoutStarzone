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

    private BuildingSO _buildingSO;

    public void SetupSlot(BuildingSO newBuildingItemSO)
    {
        _buildingSO = newBuildingItemSO;
        _icon.sprite = _buildingSO.ItemIcon;
        _icon.enabled = true;
        _useButton.interactable = true;
        _costText.text = newBuildingItemSO.Cost.ToString();
    }

    public void ClearSlot()
    {
        _buildingSO = null;
        _icon.sprite = null;
        _icon.enabled = false;
        _useButton.interactable = false;
        _costText.text = "";
    }

    // Called by clicking on inventory slot
	public void OnUseButton()
    {
        if (_buildingSO != null)
        {
            // TODO: Set Use() up in Item and its child classes
            _buildingSO.Use();
        }
    }
}