using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public static event Action OnBaseValueChanged;

    public SOStatType StatTypeSO;
    [SerializeField]
    protected int _baseValue;

    protected List<int> _modifiers = new();

    // Just to see in the inspector, variable has no real use. 
    // TODO - Delete before building, waste of resources. 
    [SerializeField]
    protected int _moddedValue;

/*    public Stat(SOStatType statSO, int baseValue = 1)
    {
        StatTypeSO = statSO;

        _baseValue = baseValue;
        _modifiers = new();
    }*/

    public int GetValue()
    {
        int finalValue = _baseValue;
        _modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void ChangeBaseValue(int amount)
    {
        _baseValue += amount;
        _moddedValue = GetValue();
        // StatManager listens. 
        OnBaseValueChanged?.Invoke();
    }

    public void AddModifier(int modifier)
    {
        _modifiers.Add(modifier);

        _moddedValue = GetValue();
    }

    public void RemoveModifier(int modifier)
    {
        _modifiers.Remove(modifier);

        _moddedValue = GetValue();
    }

    public void ClearModifiers()
    {
        _modifiers.Clear();

        _moddedValue = GetValue();
    }
}