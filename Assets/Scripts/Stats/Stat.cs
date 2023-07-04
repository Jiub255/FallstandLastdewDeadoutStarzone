using System.Collections.Generic;
using UnityEngine;

// TODO - Rework for multiple PCs. Don't use SOs? 

// Just serializable for testing, to see in inspector. 
[System.Serializable]
public class Stat
{
    public static event System.Action OnBaseValueChanged;

    protected int _baseValue;

    protected List<int> _modifiers = new();

    // Just to see in the inspector, variable has no real use. 
    // TODO - Delete before building, waste of resources. 
    [SerializeField]
    protected int _moddedValue;

    // Trying to pass in default empty stat, not sure if it works yet. 
    public Stat(int baseValue = 1, List<int> modifiers = null)
    {
        _baseValue = baseValue;
        _modifiers = new();
        if (modifiers != null)
        {
            _modifiers = modifiers;
        }
    }

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