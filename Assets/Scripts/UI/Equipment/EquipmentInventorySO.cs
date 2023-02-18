using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Inventory SO", menuName = "Scriptable Object/Equipment/Equipment Inventory SO")]
public class EquipmentInventorySO : ScriptableObject, IInventory
{
    public List<EquipmentItemAmount> EquipmentItemAmounts = new List<EquipmentItemAmount>();

    public EquipmentItemAmount GetUsableItemAmountFromItemSO(EquipmentItemSO equipmentItemSO)
    {
        foreach (EquipmentItemAmount equipmentItemAmount in EquipmentItemAmounts)
        {
            if (equipmentItemAmount.EquipmentItemSO == equipmentItemSO)
            {
                return equipmentItemAmount;
            }
        }

        return null;
    }
}