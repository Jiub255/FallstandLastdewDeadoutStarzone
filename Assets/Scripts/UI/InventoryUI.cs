using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

// Could probably use this for the BuildUI too. Just make a new build inventorySlot prefab with whatever button function on it.
// I suppose crafting too, and equipment. Just set up the slot/button prefabs and the scroll views correctly.
// At least can do an abstract class and inherit for each type of UI
public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventorySO;

    [SerializeField]
    private GameObject inventorySlot;

    [SerializeField]
    private Transform inventoryContent;

    private void OnEnable()
    {
        UIManager.onOpenedMenu += PopulateInventory;
    }

    private void OnDisable()
    {
        UIManager.onOpenedMenu -= PopulateInventory;
    }

    private void PopulateInventory()
    {
        ClearInventory();

        // Put this part in inherited classes
        foreach (ItemAmount itemAmount in inventorySO.itemAmounts)
        {
            GameObject slot = Instantiate(inventorySlot, inventoryContent);
            slot.GetComponent<Image>().sprite = itemAmount.item.itemIcon;
            slot.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = itemAmount.amount.ToString();
        }
    }

    private void ClearInventory()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }
    }
}