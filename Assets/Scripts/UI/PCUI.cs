using UnityEngine;

public class PCUI : UIRefresher
{
    // TODO: Use List of prefabs instead. Not doing PCSO's anymore, they're stupid. 
    [SerializeField]
    private PCSOListSO _availablePCSO;

    private void OnEnable()
    {
        UIManager.OnOpenedMenu += PopulateInventory;
    }

    private void OnDisable()
    {
        UIManager.OnOpenedMenu -= PopulateInventory;
    }

    private void Start()
    {
        PopulateInventory();
    }

    public override void PopulateInventory()
    {
        // Clears UI
        base.PopulateInventory();

        // Populates UI
        foreach (PCItemSO pCItemSO in _availablePCSO.PCItemSOs)
        {
            GameObject slotInstance = Instantiate(SlotPrefab, SlotParent);

            slotInstance.transform.GetComponent<PCSlot>().SetupSlot(pCItemSO);
        }
    }
}