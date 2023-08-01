using System;
using System.Collections.Generic;
using UnityEngine;

// TODO - Use this as a PCManager, and recieve Usable/Equipment item events and send them to the correct PC? 
// OR, have a separate MonoBehavoiur PCManager, and do that there, just reference this SO? 
/// <summary>
/// Holds "home" and "scavenging" SOPCData lists. Sends events when they change. 
/// </summary>
[CreateAssetMenu(menuName = "Data/SOTeamData", fileName = "Current Team SO")]
public class SOTeamData : ScriptableObject
{
	// TODO - Make the same two events for scavenging list? 
	public event Action<SOPCData> OnBeforeAddPCToHomeList;
	public event Action<SOPCData> OnBeforeRemovePCFromHomeList;

	// Not sure if these will be needed. Might use before add/remove events that send the SO to be added/removed instead. 
	/// <summary>
	/// UIPC listens, remakes PC slots in HUD. 
	/// </summary>
	public event Action OnHomeSOPCListChanged;
/*	public event Action OnScavengingSOPCListChanged;*/

	[SerializeField]
	private List<SOPCData> _homePCs;
	[SerializeField]
	private List<SOPCData> _scavengingPCs;

	public List<SOPCData> HomePCs { get { return _homePCs; } }
	public List<SOPCData> ScavengingPCs { get { return _scavengingPCs; } }
	public List <SORecipe> PossibleRecipes { get; set; }
	/// <summary>
	/// Using dictionary instead of StatValue so you can change value and get by key. 
	/// </summary>
	public Dictionary<StatType, int> IndividualPCStatMaxes { get; }
//	public Dictionary<SOPCData, PCController> PCControllerDict { get; }

	/// <summary>
	/// Use this to get the healing rate. Based on total medical skill of all PCs. 
	/// </summary>
	/// <remarks>
	/// Currently totalMedicalSkill / 1000f. 
	/// </remarks>
	public float HealingRate
	{
		get
		{
			float totalMedicalSkill = 0;
			foreach (SOPCData pcSO in HomePCs)
			{
				totalMedicalSkill += pcSO.Stats[StatType.Medical].ModdedValue;
			}
			return totalMedicalSkill / 1000f;
		}
	}


	/// <summary>
	/// Is this necessary? Or does it even do anything? Or just return the same thing you put in, <br/>
	/// unless it's not on the list? So it's kinda like a bool contains check, but returns null instead of false. 
	/// </summary>
	public SOPCData this[SOPCData pcDataSO]
    {
        get
        {
			foreach (SOPCData homePCDataSO in _homePCs)
            {
				if (homePCDataSO == pcDataSO)
                {
					return homePCDataSO;
                }
            }

			Debug.LogWarning($"No SOPCData {pcDataSO.name} found");
			return null;
        }
    }

	public void AddPCToHomeList(SOPCData sopc)
    {
		OnBeforeAddPCToHomeList?.Invoke(sopc);

		HomePCs.Add(sopc);

		OnHomeSOPCListChanged?.Invoke();
    }

	public void RemovePCFromHomeList(SOPCData sopc)
    {
		if (HomePCs.Contains(sopc))
        {
			OnBeforeRemovePCFromHomeList?.Invoke(sopc);

			HomePCs.Remove(sopc);

			OnHomeSOPCListChanged?.Invoke();
        }
        else
        {
			Debug.LogWarning($"{sopc.PCPrefab.name} not on Home SOPC list. ");
        }
    }

	public void AddPCToScavengingList(SOPCData sopc)
    {
		ScavengingPCs.Add(sopc);
//		OnScavengingSOPCListChanged?.Invoke();
    }

	public void RemovePCFromScavengingList(SOPCData sopc)
    {
		if (ScavengingPCs.Contains(sopc))
        {
			ScavengingPCs.Remove(sopc);
//			OnScavengingSOPCListChanged?.Invoke();
        }
        else
        {
			Debug.LogWarning($"{sopc.PCPrefab.name} not on Scavenging SOPC list. ");
        }
    }
}