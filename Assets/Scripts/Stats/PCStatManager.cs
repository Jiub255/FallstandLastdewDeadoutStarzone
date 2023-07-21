using System;
using UnityEngine;

// Put one on each PC. 
public class PCStatManager : MonoBehaviour
{
    public static event Action OnStatsChanged;

    private SOPCData _pcSO;
    private EquipmentManager _equipmentManager;

    protected void OnEnable()
    {
        _pcSO = GetComponentInParent<PCStateMachine>().PCSO;
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

        // TODO - Set up stats UI and UIStats script. Just show stats on the equipment UI. 
        // TODO - Heard by UIEquipment, updates UI. 
        // Heard by UIRecipes, gets new metRequirementsRecipes list. 
        OnStatsChanged?.Invoke();
    }
}