using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Manages the total stats of all PCs.
public class StatManager : MonoBehaviour
{
	[SerializeField]
	private SOCurrentTeam _currentTeamSO;

//	private Dictionary<SOStatType, int> _combinedStatTotals = new();
	private Dictionary<StatType, int> _individualPCStatMaxes = new();

    private void OnEnable()
    {
        UIRecipes.OnGetMetStatRequirementsRecipes += GetMetStatRequirementsRecipes;
    }

    private void OnDisable()
    {
        UIRecipes.OnGetMetStatRequirementsRecipes -= GetMetStatRequirementsRecipes;
    }

    public List<SORecipe> GetMetStatRequirementsRecipes(List<SORecipe> unfilteredList)
    {
        List<SORecipe> metRequirementsRecipes = new();

        GetStatTotals();

        foreach (SORecipe recipe in unfilteredList)
        {
            // Check to make sure combined stat totals and individual pc stat maxes have all the keys from the requirements first. 
            // Could delete this missing keys part before final build to improve performance, not sure if it really matters. 
            List<StatRequirement> missingKeys = recipe.MinSinglePCStatRequirements.Where(
                    entry => !_individualPCStatMaxes.ContainsKey(entry.StatType)).ToList();

            if (missingKeys.Count > 0)
            {
                Debug.LogWarning($"PCs are missing stats from {recipe.name}'s stat requirements. ");
                continue;
            }

            // Combined stat requirements, probably won't use. 
/*            List<StatRequirement> unmetRequirements = recipe.CombinedStatRequirements.Where(
                entry => _combinedStatTotals[entry.StatTypeSO] < entry.RequiredAmount).ToList();*/

            // Minimum single PC stat requirements. 
            List<StatRequirement> unmetRequirements = recipe.MinSinglePCStatRequirements.Where(
                entry => _individualPCStatMaxes[entry.StatType] < entry.RequiredAmount).ToList();

            if (unmetRequirements.Count == 0)
            {
                metRequirementsRecipes.Add(recipe);
            }
        }

        return metRequirementsRecipes;
    }

    private void GetStatTotals()
    {
		foreach (SOPCData pcSO in _currentTeamSO.HomeSOPCSList)
        {
            foreach (Stat stat in pcSO.Stats.StatList)
            {
                // Update _combinedStatTotals dictionary. 
/*                if (_combinedStatTotals.ContainsKey(stat.StatTypeSO))
                {
                    _combinedStatTotals[stat.StatTypeSO] += stat.ModdedValue;
                }
                else
                {
                    _combinedStatTotals.Add(stat.StatTypeSO, stat.ModdedValue);
                }*/

                // Update _individualPCStatMaxes dictionary. 
                if (_individualPCStatMaxes.ContainsKey(stat.StatType))
                {
                    if (stat.ModdedValue > _individualPCStatMaxes[stat.StatType])
                    {
                        _individualPCStatMaxes[stat.StatType] = stat.ModdedValue;
                    }
                }
                else
                {
                    _individualPCStatMaxes.Add(stat.StatType, stat.ModdedValue);
                }
            }
        }
    }

    /// <summary>
    /// Use this to get the healing rate, at least for now. 
    /// </summary>
    public int TotalMedicalSkill
    {
        get
        {
            int totalMedicalSkill = 0;
            foreach (SOPCData pcSO in _currentTeamSO.HomeSOPCSList)
            {
                totalMedicalSkill += pcSO.Stats[StatType.Medical].ModdedValue;
            }
            return totalMedicalSkill;
        }
    }
}