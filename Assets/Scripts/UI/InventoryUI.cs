using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
        UIManager.onOpenedInventory += PopulateInventory;
    }

    private void OnDisable()
    {
        UIManager.onOpenedInventory -= PopulateInventory;
    }

    private void PopulateInventory()
    {
        ClearInventory();

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