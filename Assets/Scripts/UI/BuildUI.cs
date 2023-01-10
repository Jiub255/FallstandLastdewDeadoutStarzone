using UnityEngine;

public class BuildUI : UIRefresher
{
    [SerializeField]
    private BuildInventorySO buildInventorySO;

    private void OnEnable()
    {
        UIManager.onOpenedMenu += PopulateInventory;
    }

    private void OnDisable()
    {
        UIManager.onOpenedMenu -= PopulateInventory;
    }

    public override void PopulateInventory()
    {
        base.PopulateInventory();

        foreach (BuildingItem buildingItem in buildInventorySO.buildItems)
        {
            GameObject slotInstance = Instantiate(slotPrefab, content);
            
            slotInstance.transform.GetComponent<BuildSlot>().SetupSlot(buildingItem);
        }
    }
}