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
    /// Heard by UIRecipes, gets new metRequirementsRecipes list.
    /// </summary>
    public static event Action OnStatsChanged;

    private SOPCData _pcSO;
    private Dictionary<StatType, int> _equipmentBonuses;

    public PCStatManager(SOPCData pcDataSO, Dictionary<StatType, int> equipmentBonuses)
    {
        _pcSO = pcDataSO;
        _equipmentBonuses = equipmentBonuses;

        SetupStartingStats();

        CalculateStatModifiers();
    }

    /// <summary>
    /// Make a new list of stats in SOPCData based on presets in SOPCSharedData. <br/>
    /// Also ensures you have a complete list of stats, instead of setting it up on each character individually. 
    /// </summary>
    private void SetupStartingStats()
    {
        _pcSO.Stats.StatList.Clear();
        foreach(Stat stat in _pcSO.PCSharedDataSO.StartingStats.StatList)
        {
            _pcSO.Stats.StatList.Add(new Stat(stat.StatType, stat.ModdedValue, this));
        }
    }

    /// <summary>
    /// Calculates stat modifiers from equipment, then updates UICharacter. 
    /// </summary>
    public void CalculateStatModifiers()
    {
        // Keep the bonuses dictionary in EquipmentManager, and change it every time
        // equipment is changed. Then it's just ready and you can grab it whenever. 
        foreach (Stat stat in _pcSO.Stats.StatList)
        {
            stat.ClearModifiers();

            if (_equipmentBonuses.ContainsKey(stat.StatType))
            {
                stat.AddModifier(_equipmentBonuses[stat.StatType]);
            }

            SubtractPainPenalty(stat);
        }

        OnStatsChanged?.Invoke();
    }

    /// <summary>
    /// TESTING - Subtract from stat based on pain.
    /// </summary>
    /// <param name="stat"></param>
    private void SubtractPainPenalty(Stat stat)
    {
        // Integer division truncates. 
        int painPenalty = (-1 * _pcSO.Pain) / 10;
        stat.AddModifier(painPenalty);
    }
}