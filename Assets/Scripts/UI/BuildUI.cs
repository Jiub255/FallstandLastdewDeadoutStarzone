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

        foreach (BuildingItemSO buildingItemSO in _buildInventorySO.BuildItems)
        {
            GameObject slotInstance = Instantiate(SlotPrefab, Content);
            
            slotInstance.transform.GetComponent<BuildSlot>().SetupSlot(buildingItemSO);
        }
    }
}