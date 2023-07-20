using System;
using UnityEngine;

// Put one on each PC. 
public class PCStatManager : MonoBehaviour
{
    public static event Action OnStatsChanged;

    private SOPC _pcSO;
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
        for (int i = 0; i < _pcSO.StatsSerDict.Count(); i++)
        {
            _pcSO.StatsSerDict[i].Value.ClearModifiers();

            if (_equipmentManager.EquipmentBonuses.ContainsKey(_pcSO.StatsSerDict[i].Value.StatType))
            {
                _pcSO.StatsSerDict[i].Value.AddModifier(_equipmentManager.EquipmentBonuses[_pcSO.StatsSerDict[i].Value.StatType]);
            }
        }
/*        foreach (SKVP<string, Stat> skvp in _pcSO.StatsSerDict)
        {
            skvp.ClearModifiers();

            if (_equipmentManager.EquipmentBonuses.ContainsKey(skvp.StatType))
            {
                skvp.AddModifier(_equipmentManager.EquipmentBonuses[skvp.StatType]);
            }
        }*/

        // TODO - Set up stats UI and UIStats script. Just show stats on the equipment UI. 
        // TODO - Heard by UIEquipment, updates UI. 
        // Heard by UIRecipes, gets new metRequirementsRecipes list. 
        OnStatsChanged?.Invoke();
    }
}