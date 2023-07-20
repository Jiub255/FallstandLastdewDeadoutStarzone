using System;
using System.Collections.Generic;

// Just serializable for testing, to see in inspector. 
//[Serializable]
public class Equipment
{
/*    public static event Action<SOEquipmentItem> OnUnequip;
    public event Action OnEquipmentChanged;

    // TODO - Set up a PC equipment SO instead of this? Then just split this class between EquipmentManager and the SO. 
    // But then would need one for each PC. Could just create them through code? 
    public List<SOEquipmentItem> EquipmentItems;

    public Equipment()
    {
        EquipmentItems = new();
    }

    public void Equip(SOEquipmentItem newItem)
    {
        EquipmentType type = newItem.EquipmentType;

        // If there is something equipped in this slot, unequip it. 
        for (int i = 0; i < EquipmentItems.Count; i++)
        {
            if (type == EquipmentItems[i].EquipmentType)
            {
                SOEquipmentItem oldItem = EquipmentItems[i];

                Unequip(oldItem);
            }
        }

        // Add new item to EquipmentItems. 
        EquipmentItems.Add(newItem);

        // UIEquipment and StatManager listen. 
        OnEquipmentChanged?.Invoke();
    }

    public void Unequip(SOEquipmentItem oldItem)
    {
        // Remove old item from EquipmentItems. 
        EquipmentItems.Remove(oldItem);

        // UIEquipment and StatManager listen. 
        OnEquipmentChanged?.Invoke();

        // InventoryManager listens.
        OnUnequip?.Invoke(oldItem);
    }*/
}