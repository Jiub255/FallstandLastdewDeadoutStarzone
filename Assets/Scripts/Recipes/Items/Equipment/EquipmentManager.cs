using System;
using System.Collections.Generic;

public class EquipmentManager
{
    /// <summary>
    /// Heard by PlayerInventoryManager, removes item from inventory. 
    /// </summary>
    public static event Action<SOEquipmentItem> OnEquip;
    /// <summary>
    /// Heard by PlayerInventoryManager, adds item back into inventory. 
    /// </summary>
    public static event Action<SOEquipmentItem> OnUnequip;
    /// <summary>
    /// Heard by PCStatManager, recalculates stat modifiers. 
    /// </summary>
    public event Action OnEquipmentChanged;

    private SOPCData PCDataSO { get; }
    /// <summary>
    /// Keep a list of total equipment bonuses for each stat from all equipment for easy access. 
    /// </summary>
    public Dictionary<StatType, int> EquipmentBonuses { get; }

    public EquipmentManager(SOPCData pcDataSO)
    {
        PCDataSO = pcDataSO;

        EquipmentBonuses = new();
    }

    /// <summary>
    /// Unequips item if there is already one of the same equipment type, <br/>
    /// adds item to Equipment list, <br/>
    /// calculates equipment bonuses with new equipment, <br/>
    /// removes item from inventory, <br/>
    /// and recalculates stat modifiers last. 
    /// </summary>
    /// <param name="equipmentItemSO"></param>
    public void Equip(SOEquipmentItem equipmentItemSO)
    {
        // If there is something equipped of the same equipment type, unequip it. 
        if (PCDataSO.Equipment[equipmentItemSO.EquipmentType] != null)
        {
            SOEquipmentItem oldItem = PCDataSO.Equipment[equipmentItemSO.EquipmentType];

            Unequip(oldItem);
        }

        PCDataSO.Equipment.Add(equipmentItemSO);

        // Has to be before OnEquipmentChanged, so stats can be properly calculated from equipment bonuses. 
        CalculateEquipmentBonuses();

        OnEquip?.Invoke(equipmentItemSO);
        OnEquipmentChanged?.Invoke();
    }

    /// <summary>
    /// Removes from Equipment list, <br/>
    /// calculates equipment bonuses with new equipment, <br/>
    /// adds item to inventory, <br/>
    /// and recalculates stat modifiers last. 
    /// </summary>
    /// <param name="equipmentItemSO"></param>
    public void Unequip(SOEquipmentItem equipmentItemSO)
    {
        PCDataSO.Equipment.Remove(equipmentItemSO);

        // Has to be before OnEquipmentChanged, so stats can be properly calculated from equipment bonuses. 
        CalculateEquipmentBonuses();

        OnUnequip?.Invoke(equipmentItemSO);
        OnEquipmentChanged?.Invoke();
    }

    private void CalculateEquipmentBonuses()
    {
        EquipmentBonuses.Clear();

        foreach (SOEquipmentItem equipmentItemSO in PCDataSO.Equipment.EquipmentItems)
        {
            foreach (EquipmentBonus equipmentBonus in equipmentItemSO.Bonuses)
            {
                if (EquipmentBonuses.ContainsKey(equipmentBonus.StatType))
                {
                    EquipmentBonuses[equipmentBonus.StatType] += equipmentBonus.BonusAmount;
                }
                else
                {
                    EquipmentBonuses.Add(equipmentBonus.StatType, equipmentBonus.BonusAmount);
                }
            }
        }
    }
}