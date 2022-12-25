using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIRefresher : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventorySO;

    public GameObject slotPrefab;

    public Transform content;

    private void OnEnable()
    {
        UIManager.onOpenedMenu += PopulateInventory;
    }

    private void OnDisable()
    {
        UIManager.onOpenedMenu -= PopulateInventory;
    }

    public void PopulateInventory()
    {
        ClearInventory();

        foreach (ItemAmount itemAmount in inventorySO.itemAmounts)
        {
            GameObject slot = Instantiate(slotPrefab, content);
            slot.GetComponent<Image>().sprite = itemAmount.item.itemIcon;
            slot.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = itemAmount.amount.ToString();
        }
    }

    private void ClearInventory()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}