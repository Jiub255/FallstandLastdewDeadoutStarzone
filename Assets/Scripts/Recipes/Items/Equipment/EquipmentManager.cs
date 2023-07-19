using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static event Action<SOEquipmentItem> OnUnequip;
    public event Action OnEquipmentChanged;

    // So StatManager can get equipment list. 
    [SerializeField]
    private SOPC _pcSO;

    public Dictionary<SOStatType, int> EquipmentBonuses { get; private set; } = new();

    private void OnEnable()
    {
        SOEquipmentItem.OnEquip += Equip;
        SOEquipmentItem.OnUnequip += Unequip;

        if (_pcSO == null)
        {
            Debug.LogWarning($"No SOPC found on {transform.root.name}");
        }
    }

    private void OnDisable()
    {
        SOEquipmentItem.OnEquip -= Equip;
        SOEquipmentItem.OnUnequip -= Unequip;
    }

    // TODO - As is, this will equip the item on every PC. How to make it only the MenuSelected one?
    // Could pass an id parameter in the static event and have all EquipmentManagers check. 
    // But that seems ugly and there's probably a better way. 
    // Could just subscribe only when selected, so only the selected instance gets the event. 
    // Then unsubscribe when deselected (and still OnDisable too). 
    public void Equip(SOEquipmentItem equipmentItemSO)
    {
//        PCSO.Equip(equipmentItemSO);

        SOEquipmentType type = equipmentItemSO.EquipmentType;

        // If there is something equipped in this slot, unequip it. 
        for (int i = 0; i < _pcSO.EquipmentItems.Count; i++)
        {
            if (type.name == _pcSO.EquipmentItems[i].EquipmentType.name)
            {
                SOEquipmentItem oldItem = _pcSO.EquipmentItems[i];

                Unequip(oldItem);
            }
        }

        // Add new item to EquipmentItems. 
        _pcSO.EquipmentItems.Add(equipmentItemSO);

        // UIEquipment and StatManager listen. 
        OnEquipmentChanged?.Invoke();

        AddEquipmentBonuses(equipmentItemSO);
    }

    private void AddEquipmentBonuses(SOEquipmentItem equipmentItemSO)
    {
        foreach (EquipmentBonus equipmentBonus in equipmentItemSO.Bonuses)
        {
            if (EquipmentBonuses.ContainsKey(equipmentBonus.StatTypeSO))
            {
                EquipmentBonuses[equipmentBonus.StatTypeSO] += equipmentBonus.BonusAmount;
            }
            else
            {
                EquipmentBonuses.Add(equipmentBonus.StatTypeSO, equipmentBonus.BonusAmount);
            }
        }
    }

    public void Unequip(SOEquipmentItem equipmentItemSO)
    {
//        PCSO.Unequip(equipmentItemSO);

        // Remove old item from EquipmentItems. 
        _pcSO.EquipmentItems.Remove(equipmentItemSO);

        // UIEquipment (not yet, TODO ) and SOStatManager listen. 
        OnEquipmentChanged?.Invoke();

        // InventoryManager listens.
        OnUnequip?.Invoke(equipmentItemSO);

        RemoveEquipmentBonuses(equipmentItemSO);
    }

    private void RemoveEquipmentBonuses(SOEquipmentItem equipmentItemSO)
    {
        foreach (EquipmentBonus equipmentBonus in equipmentItemSO.Bonuses)
        {
            if (EquipmentBonuses.ContainsKey(equipmentBonus.StatTypeSO))
            {
                EquipmentBonuses[equipmentBonus.StatTypeSO] -= equipmentBonus.BonusAmount;
                if (EquipmentBonuses[equipmentBonus.StatTypeSO] < 0)
                {
                    Debug.LogWarning($"{equipmentBonus.StatTypeSO}'s equipment bonus is below zero, this should never happen. ");
                    EquipmentBonuses.Remove(equipmentBonus.StatTypeSO);
                }
                else if (EquipmentBonuses[equipmentBonus.StatTypeSO] == 0)
                {
                    EquipmentBonuses.Remove(equipmentBonus.StatTypeSO);
                }
            }
            else
            {
                Debug.LogWarning($"Key {equipmentBonus.StatTypeSO} not found on _equipmentBonuses. This should never happen. ");
            }
        }
    }
}