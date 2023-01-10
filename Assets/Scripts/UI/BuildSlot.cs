using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildSlot : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Button useButton;
    [SerializeField]
    private TextMeshProUGUI costText;

    // Need to have a separate list of Build Items, separate inv/invSO too I think.
    // Makes sense, they're very different things, inv and buildinv.
    private BuildingItem buildingItem;

    public void SetupSlot(BuildingItem newBuildingItem)
    {
        buildingItem = newBuildingItem;
        icon.sprite = buildingItem.itemIcon;
        icon.enabled = true;
        useButton.interactable = true;
        costText.text = newBuildingItem.cost.ToString();
    }

    public void ClearSlot()
    {
        buildingItem = null;
        icon.sprite = null;
        icon.enabled = false;
        useButton.interactable = false;
        costText.text = "";
    }

    // Called by clicking on inventory slot
	public void OnUseButton()
    {
        if (buildingItem != null)
        {
            // TODO: Set Use() up in Item and its child classes
            //inventoryItem.Use();
        }
    }
}