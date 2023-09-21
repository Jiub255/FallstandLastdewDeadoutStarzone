using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the total stats of all PCs.
/// </summary>
public class StatManager
{
	private SOTeamData TeamDataSO { get; set; }

    public StatManager(SOTeamData teamDataSO)
    {
        TeamDataSO = teamDataSO;
    }

    /// <summary>
    /// Filters out all of the SORecipes that you don't meet the stat requirements for. 
    /// </summary>
    public List<T> GetMetStatRequirementsRecipes<T>(List<T> unfilteredList) where T : SORecipe
    {
        GetStatTotals();

        Debug.Log($"Pre stat filtered list count: {unfilteredList.Count}");

        // Does this fancy LINQ work? 
        List<T> filteredList = unfilteredList
            .Where(recipeSO => recipeSO.MinSinglePCStatRequirements
            .Where(statRequirement => !TeamDataSO.IndividualPCStatMaxes
            .ContainsKey(statRequirement.StatType) ||
            TeamDataSO.IndividualPCStatMaxes[statRequirement.StatType] < statRequirement.Value)
            .ToList().Count == 0)
            .ToList();

        Debug.Log($"Post stat filtered list count: {filteredList.Count}");

        return filteredList;


/*        List<T> metRequirementsRecipes = new();

        foreach (T recipe in unfilteredList)
        {
            // Check to make sure combined stat totals and individual pc stat maxes have all the keys from the requirements first. 
            // Could delete this missing keys part before final build to improve performance, not sure if it really matters. 
            List<StatValue> missingKeys = recipe.MinSinglePCStatRequirements.Where(
                    entry => !GameDataSO.IndividualPCStatMaxes.ContainsKey(entry.StatType)).ToList();

            if (missingKeys.Count > 0)
            {
                Debug.LogWarning($"PCs are missing stats from {recipe.name}'s stat requirements. ");
                continue;
            }

            // Combined stat requirements, probably won't use. 
*//*            List<StatRequirement> unmetRequirements = recipe.CombinedStatRequirements.Where(
                entry => _combinedStatTotals[entry.StatType] < entry.RequiredAmount).ToList();*//*
            
            // Minimum single PC stat requirements. 
            List<StatValue> unmetRequirements = recipe.MinSinglePCStatRequirements.Where(
                entry => GameDataSO.IndividualPCStatMaxes[entry.StatType] < entry.Value).ToList();

            if (unmetRequirements.Count == 0)
            {
                metRequirementsRecipes.Add(recipe);
            }
        }

        return metRequirementsRecipes;*/
    }

    private void GetStatTotals()
    {
		foreach (SOPCData pcSO in TeamDataSO.HomePCs)
        {
            foreach (Stat stat in pcSO.Stats)
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
                if (TeamDataSO.IndividualPCStatMaxes.ContainsKey(stat.StatType))
                {
                    if (stat.ModdedValue > TeamDataSO.IndividualPCStatMaxes[stat.StatType])
                    {
                        TeamDataSO.IndividualPCStatMaxes[stat.StatType] = stat.ModdedValue;
                    }
                }
                else
                {
                    TeamDataSO.IndividualPCStatMaxes.Add(stat.StatType, stat.ModdedValue);
                }
            }
        }
    }

/*    /// <summary>
    /// Use this to get the healing rate, at least for now.
    /// TODO - Where to put this? On CurrentTeamSO probably. 
    /// </summary>
    public int TotalMedicalSkill
    {
        get
        {
            int totalMedicalSkill = 0;
            foreach (SOPCData pcSO in _gameDataSO.CurrentTeamSO.HomeSOPCSList)
            {
                totalMedicalSkill += pcSO.Stats[StatType.Medical].ModdedValue;
            }
            return totalMedicalSkill;
        }
    }*/
}