using UnityEngine;

public class BuildUI : UIRefresher
{
    [SerializeField]
    private BuildInventorySO _buildInventorySO;

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

        foreach (BuildItemSO buildingItemSO in _buildInventorySO.BuildItemSOs)
        {
            GameObject slotInstance = Instantiate(SlotPrefab, SlotParent);
            
            slotInstance.transform.GetComponent<BuildSlot>().SetupSlot(buildingItemSO);
        }
    }
}