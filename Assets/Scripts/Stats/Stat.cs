using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Attack,
    Defense, 
    Scavenging, 
    Medical, 
    Engineering, 
    Farming
}

[Serializable]
public class Stat
{
    /// <summary>
    /// Heard by PCStatManager, recalculates stat modifiers. 
    /// </summary>
    public static event Action OnBaseValueChanged;

    [SerializeField]
    private StatType _statType;
    [SerializeField]
    private int _baseValue;

    private List<int> _modifiers;
    private int _moddedValue;

    public StatType StatType { get { return _statType; } }
    public int ModdedValue { get { return _moddedValue; } }

    private void CalculateModdedValue()
    {
        int finalValue = _baseValue;
        _modifiers.ForEach(x => finalValue += x);
        _moddedValue = finalValue;
    }

    public void ChangeBaseValue(int amount)
    {
        _baseValue += amount;
        CalculateModdedValue();

        OnBaseValueChanged?.Invoke();
    }

    public void AddModifier(int modifier)
    {
        _modifiers.Add(modifier);
        CalculateModdedValue();
    }

    public void ClearModifiers()
    {
        _modifiers.Clear();
        CalculateModdedValue();
    }
}