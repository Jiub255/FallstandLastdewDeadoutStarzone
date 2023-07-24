using System;
using System.Collections.Generic;
using UnityEngine;

// TODO - Make this a plain C# class, and pass the SOPCData in the constructor? No reason for it to be a MB.
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

    private SOPCData _pcSO;
    private Dictionary<StatType, int> _equipmentBonuses;

    /// <summary>
    /// Keep a list of total equipment bonuses for each stat from all equipment for easy access. 
    /// </summary>
    public Dictionary<StatType, int> EquipmentBonuses { get { return _equipmentBonuses; } }

    public EquipmentManager(SOPCData pcDataSO)
    {
        _pcSO = pcDataSO;

        _equipmentBonuses = new();
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
        if (_pcSO.Equipment[equipmentItemSO.EquipmentType] != null)
        {
            SOEquipmentItem oldItem = _pcSO.Equipment[equipmentItemSO.EquipmentType];

            Unequip(oldItem);
        }

        _pcSO.Equipment.Add(equipmentItemSO);

        // Has to be before OnEquipmentChanged, so stats can be properly calculated from equipment bonuses. 
        CalculateEquipmentBonuses();
//        AddEquipmentBonuses(equipmentItemSO);

        OnEquip?.Invoke(equipmentItemSO);
        OnEquipmentChanged?.Invoke();
    }

    private void CalculateEquipmentBonuses()
    {
        EquipmentBonuses.Clear();

        foreach (SOEquipmentItem equipmentItemSO in _pcSO.Equipment.EquipmentItems)
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

    /// <summary>
    /// Removes from Equipment list, <br/>
    /// calculates equipment bonuses with new equipment, <br/>
    /// adds item to inventory, <br/>
    /// and recalculates stat modifiers last. 
    /// </summary>
    /// <param name="equipmentItemSO"></param>
    public void Unequip(SOEquipmentItem equipmentItemSO)
    {
        _pcSO.Equipment.Remove(equipmentItemSO);

        CalculateEquipmentBonuses();
//        RemoveEquipmentBonuses(equipmentItemSO);

        OnUnequip?.Invoke(equipmentItemSO);
        OnEquipmentChanged?.Invoke();
    }

/*    private void AddEquipmentBonuses(SOEquipmentItem equipmentItemSO)
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
    }*/

/*    private void RemoveEquipmentBonuses(SOEquipmentItem equipmentItemSO)
    {
        foreach (EquipmentBonus equipmentBonus in equipmentItemSO.Bonuses)
        {
            if (EquipmentBonuses.ContainsKey(equipmentBonus.StatType))
            {
                EquipmentBonuses[equipmentBonus.StatType] -= equipmentBonus.BonusAmount;
                if (EquipmentBonuses[equipmentBonus.StatType] < 0)
                {
                    Debug.LogWarning($"{equipmentBonus.StatType}'s equipment bonus is below zero, this should never happen. ");
                    EquipmentBonuses.Remove(equipmentBonus.StatType);
                }
                else if (EquipmentBonuses[equipmentBonus.StatType] == 0)
                {
                    EquipmentBonuses.Remove(equipmentBonus.StatType);
                }
            }
            else
            {
                Debug.LogWarning($"Key {equipmentBonus.StatType} not found on _equipmentBonuses. This should never happen. ");
            }
        }
    }*/
}