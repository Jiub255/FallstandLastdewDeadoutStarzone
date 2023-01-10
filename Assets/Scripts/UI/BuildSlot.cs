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
    private BuildingItemSO buildingItemSO;

    public void SetupSlot(BuildingItemSO newBuildingItemSO)
    {
        buildingItemSO = newBuildingItemSO;
        icon.sprite = buildingItemSO.itemIcon;
        icon.enabled = true;
        useButton.interactable = true;
        costText.text = newBuildingItemSO.cost.ToString();
    }

    public void ClearSlot()
    {
        buildingItemSO = null;
        icon.sprite = null;
        icon.enabled = false;
        useButton.interactable = false;
        costText.text = "";
    }

    // Called by clicking on inventory slot
	public void OnUseButton()
    {
        if (buildingItemSO != null)
        {
            // TODO: Set Use() up in Item and its child classes
            buildingItemSO.Use();
        }
    }
}