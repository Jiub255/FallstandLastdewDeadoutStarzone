using System;
using UnityEngine;

// TODO - Make this a plain C# class, and pass the EquipmentManager in the constructor? No reason for it to be a MB.
// Maybe even combine it with Stats class?
public class PCStatManager : MonoBehaviour
{
    /// <summary>
    /// Heard by UICharacter, updates UI. Heard by UIRecipes, gets new metRequirementsRecipes list. 
    /// </summary>
public static event Action OnStatsChanged;

    private SOPCData _pcSO;
    private EquipmentManager _equipmentManager;

    protected void OnEnable()
    {
        _pcSO = GetComponentInParent<PCController>().PCSO;
        _equipmentManager = transform.parent.GetComponentInChildren<EquipmentManager>();

        _equipmentManager.OnEquipmentChanged += CalculateStatModifiers;
        Stat.OnBaseValueChanged += CalculateStatModifiers;

        CalculateStatModifiers();
    }

    protected void OnDisable()
    {
        _equipmentManager.OnEquipmentChanged -= CalculateStatModifiers;
        Stat.OnBaseValueChanged -= CalculateStatModifiers;
    }

    protected void CalculateStatModifiers()
    {
        // Keep the bonuses dictionary in EquipmentManager, and change it every time
        // equipment is changed. Then it's just ready and you can grab it whenever. 
        foreach (Stat stat in _pcSO.Stats.StatList)
        {
            stat.ClearModifiers();

            if (_equipmentManager.EquipmentBonuses.ContainsKey(stat.StatType))
            {
                stat.AddModifier(_equipmentManager.EquipmentBonuses[stat.StatType]);
            }
        }

        OnStatsChanged?.Invoke();
    }
}