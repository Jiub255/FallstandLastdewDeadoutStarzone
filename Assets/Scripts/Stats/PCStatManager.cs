using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO - Use _pcSO.Pain to affect stats. The higher the pain, the lower the stats. Work out a formula/formulas. 
/// </summary>
public class PCStatManager
{
    /// <summary>
    /// Heard by UICharacter, updates UI. <br/>
    /// Heard by UIRecipes, gets new metRequirementsRecipes list. <br/>
    /// Heard by GameManager, updates possible recipe lists. 
    /// </summary>
    public static event Action OnStatsChanged;

    private SOPCData PCDataSO { get; }
    private Dictionary<StatType, int> EquipmentBonuses { get; }

    public PCStatManager(SOPCData pcDataSO, Dictionary<StatType, int> equipmentBonuses)
    {
        PCDataSO = pcDataSO;
        EquipmentBonuses = equipmentBonuses;

//        SetupStartingStats();

        SubscribeToStatEvents();

        CalculateStatModifiers();
    }

    private void SubscribeToStatEvents()
    {
        foreach (Stat stat in PCDataSO.Stats)
        {
            stat.OnBaseValueChanged += CalculateStatModifiers;
        }
    }

    public void OnDisable()
    {
        foreach (Stat stat in PCDataSO.Stats)
        {
            stat.OnBaseValueChanged -= CalculateStatModifiers;
        }
    }

    /// <summary>
    /// Make a new list of stats in SOPCData based on presets in SOPCSharedData. <br/>
    /// Also ensures you have a complete list of stats, instead of setting it up on each character individually. <br/>
    /// TODO - How to not reset PC's stats on every new scene/load? 
    /// </summary>
/*    private void SetupStartingStats()
    {
        PCDataSO.Stats.StatList.Clear();
        foreach(Stat stat in PCDataSO.PCSharedDataSO.StartingStats.StatList)
        {
            PCDataSO.Stats.StatList.Add(new Stat(stat.StatType, stat.BaseValue, this));
        }
    }*/

    /// <summary>
    /// Calculates stat modifiers from equipment, then updates UICharacter. 
    /// </summary>
    public void CalculateStatModifiers()
    {
        // Keep the bonuses dictionary in EquipmentManager, and change it every time
        // equipment is changed. Then it's just ready and you can grab it whenever. 
        foreach (Stat stat in PCDataSO.Stats)
        {
            stat.ClearModifiers();

            if (EquipmentBonuses.ContainsKey(stat.StatType))
            {
                stat.AddModifier(EquipmentBonuses[stat.StatType]);
            }

            SubtractPainPenalty(stat);
        }

        OnStatsChanged?.Invoke();
    }

    /// <summary>
    /// TESTING - Subtract from stat based on pain. <br/>
    /// Maybe do this separately instead? So it can change frame by frame without needing to recalculate all the other stat modifiers each time. 
    /// </summary>
    private void SubtractPainPenalty(Stat stat)
    {
        int painPenalty = (-1 * PCDataSO.Pain) / 10;
        stat.AddModifier(painPenalty);
    }
}