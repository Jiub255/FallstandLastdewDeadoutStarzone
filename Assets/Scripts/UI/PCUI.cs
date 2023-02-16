using UnityEngine;

public class PCUI : UIRefresher
{
    [SerializeField]
    private GOListSO _pcInstancesSO;

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
        foreach (GameObject pcInstance in _pcInstancesSO.GameObjects)
        {
            GameObject slotInstance = Instantiate(SlotPrefab, SlotParent);

            slotInstance.transform.GetComponent<PCSlot>().SetupSlot(pcInstance);
        }
    }
}