using UnityEngine;

public class EquipmentUI : UIRefresher
{
    [SerializeField]
    private EquipmentInventorySO _equipmentInventorySO;

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

        foreach (EquipmentItemAmount equipmentItemAmount in _equipmentInventorySO.EquipmentItemAmounts)
        {
            GameObject slotInstance = Instantiate(SlotPrefab, SlotParent);

            slotInstance.transform.GetComponent<EquipmentSlot>().SetupSlot(equipmentItemAmount);
        }
    }
}