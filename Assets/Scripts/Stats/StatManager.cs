using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Manages the total stats of all PCs.
public class StatManager : MonoBehaviour
{
	[SerializeField]
	private SOListSOPC _pcSOListSO;

	private List<StatManager> _statManagers = new();
	private Dictionary<SOStatType, int> _combinedStatTotals = new();
	private Dictionary<SOStatType, int> _individualPCStatMaxes = new();

    private void OnEnable()
    {
        UIRecipes.OnGetMetRequirementsRecipes += GetMetRequirementsRecipes;
    }

    private void OnDisable()
    {
        UIRecipes.OnGetMetRequirementsRecipes -= GetMetRequirementsRecipes;
    }

    public List<SORecipe> GetMetRequirementsRecipes(List<SORecipe> unfilteredList)
    {
        List<SORecipe> metRequirementsRecipes = new();

        GetStatTotals();

        foreach (SORecipe recipe in unfilteredList)
        {
            // Check to make sure combined stat totals and individual pc stat maxes have all the keys from the requirements first. 
            // Could delete this missing keys part before final build to improve performance, not sure if it really matters. 
            List<StatRequirement> missingKeys = 
                recipe.CombinedStatRequirements.Where(
                    entry => !_combinedStatTotals.ContainsKey(entry.StatTypeSO))
                    .ToList();

            if (missingKeys.Count > 0)
            {
                Debug.LogWarning($"PCs are missing stats from {recipe.name}'s stat requirements. ");
                continue;
            }

            List<StatRequirement> unmetRequirements =
                recipe.CombinedStatRequirements.Where(
                entry => _combinedStatTotals[entry.StatTypeSO] < entry.RequiredAmount)
                .ToList();

            if (unmetRequirements.Count == 0)
            {
                metRequirementsRecipes.Add(recipe);
            }
        }

        return metRequirementsRecipes;
    }

    private void GetStatTotals()
    {
		foreach (SOPC pcSO in _pcSOListSO.HomeSOPCSList)
        {
            foreach (Stat stat in pcSO.PCInstance.GetComponent<PCStatManager>().Stats)
            {
                // Update _combinedStatTotals dictionary. 
                if (_combinedStatTotals.ContainsKey(stat.StatTypeSO))
                {
                    _combinedStatTotals[stat.StatTypeSO] += stat.ModdedValue;
                }
                else
                {
                    _combinedStatTotals.Add(stat.StatTypeSO, stat.ModdedValue);
                }

                // Update _individualPCStatMaxes dictionary. 
                if (_individualPCStatMaxes.ContainsKey(stat.StatTypeSO))
                {
                    if (stat.ModdedValue > _individualPCStatMaxes[stat.StatTypeSO])
                    {
                        _individualPCStatMaxes[stat.StatTypeSO] = stat.ModdedValue;
                    }
                }
                else
                {
                    _individualPCStatMaxes.Add(stat.StatTypeSO, stat.ModdedValue);
                }
            }
        }
    }
}