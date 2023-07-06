using UnityEngine;

// Controls switching tabs (Usable, Crafting, and Equipment) in inventory. 
public class UITabs : MonoBehaviour
{
	[SerializeField]
	private GameObject _usableItemsPanel;
	[SerializeField]
	private GameObject _equipmentItemsPanel;
	[SerializeField]
	private GameObject _craftingItemsPanel;

	public void OpenUsableItems()
    {
		CloseAllTabs();
		if (!_usableItemsPanel.activeInHierarchy) _usableItemsPanel.SetActive(true);
	}

	public void OpenEquipment()
    {
		CloseAllTabs();
		if (!_equipmentItemsPanel.activeInHierarchy) _equipmentItemsPanel.SetActive(true);
	}

	public void OpenCraftingItems()
    {
		CloseAllTabs();
		if (!_craftingItemsPanel.activeInHierarchy) _craftingItemsPanel.SetActive(true);
	}

	private void CloseAllTabs()
    {
		if (_usableItemsPanel.activeInHierarchy) _usableItemsPanel.SetActive(false);
		if (_equipmentItemsPanel.activeInHierarchy) _equipmentItemsPanel.SetActive(false);
		if (_craftingItemsPanel.activeInHierarchy) _craftingItemsPanel.SetActive(false);
    }
}