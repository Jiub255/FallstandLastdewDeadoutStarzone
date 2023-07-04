using UnityEngine;

public class UsableUI : UIRefresher
{
    [SerializeField]
    private UsableInventorySO _usableInventorySO;

    private void OnEnable()
    {
        UIManager.OnOpenedMenu += PopulateInventory;
    }

    private void OnDisable()
    {
        UIManager.OnOpenedMenu -= PopulateInventory;
    }

    public override void PopulateInventory()
    {
        base.PopulateInventory();

        foreach (UsableItemAmount usableItemAmount in _usableInventorySO.UsableItemAmounts)
        {
            GameObject slotInstance = Instantiate(SlotPrefab, SlotParent);

            slotInstance.transform.GetComponent<UsableItemSlot>().SetupSlot(usableItemAmount);
        }
    }
}