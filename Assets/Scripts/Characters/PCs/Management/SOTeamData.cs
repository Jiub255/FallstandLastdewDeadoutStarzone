using System;
using System.Collections.Generic;
using System.Linq;
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
	/// UIPCHUD listens, remakes PC slots in HUD. 
	/// </summary>
	public event Action OnHomeSOPCListChanged;
/*	public event Action OnScavengingSOPCListChanged;*/

	[SerializeField]
	private List<SOPCData> _homePCs;
	[SerializeField]
	private List<SOPCData> _scavengingPCs;
	[SerializeField]
	private SOPCDatabase _pcDatabaseSO;
	[SerializeField]
	private SOItemDatabase _itemDatabaseSO;

	public List<SOPCData> HomePCs { get { return _homePCs; } }
	public List<SOPCData> ScavengingPCs { get { return _scavengingPCs; } }
	public SOPCDatabase PCDatabaseSO { get { return _pcDatabaseSO; } }
	public SOItemDatabase ItemDatabaseSO { get { return _itemDatabaseSO; } }
	public List <SORecipe> PossibleRecipes { get; set; }
	/// <summary>
	/// Using dictionary instead of StatValue so you can change value and get by key. 
	/// </summary>
	public Dictionary<StatType, int> IndividualPCStatMaxes { get; } = new();

	/// <summary>
	/// Use this to get the healing rate. Based on total medical skill of all PCs. <br/>
	/// TODO - Get this on load, and any time that the total medical skill changes and just cache the value. 
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

	public void SaveData(GameSaveData gameData)
	{
		// Save Home PCs list. 
		gameData.HomePCs.Clear();
		foreach (SOPCData pcDataSO in HomePCs)
        {
			// Save individual PC's data.
			int id = PCDatabaseSO.PCDataSOs.IndexOf(pcDataSO);

			int injury = pcDataSO.Injury;

			List<(StatType, int)> stats = pcDataSO.Stats.StatList
				.Select(x => (x.StatType, x.BaseValue))
				.ToList();

			// To load equipment, just get all the items from database, and have EquipmentManager equip them. 
			List<int> equipmentIDs = pcDataSO.Equipment.EquipmentItems
				.Select(x => ItemDatabaseSO.Items.IndexOf(x))
				.ToList();

			PCSaveData pcSaveData = new(id, injury, stats, equipmentIDs);

			gameData.HomePCs.Add(pcSaveData);
        }

		// Save Scavenging PCs list. 
		gameData.ScavengingPCIDs.Clear();
		foreach (SOPCData pcDataSO in ScavengingPCs)
        {
			int id = PCDatabaseSO.PCDataSOs.IndexOf(pcDataSO);
			gameData.ScavengingPCIDs.Add(id);
        }
	}

	public void LoadData(GameSaveData gameData)
    {
		// Load HomePCs list. 
		HomePCs.Clear();
		foreach (PCSaveData pcSaveData in gameData.HomePCs)
        {
			// Injury
			PCDatabaseSO.PCDataSOs[pcSaveData.PCID].Injury = pcSaveData.Injury;

			// Stats
			PCDatabaseSO.PCDataSOs[pcSaveData.PCID].Stats.StatList.Clear();
			foreach ((StatType, int) tuple in pcSaveData.Stats)
            {
				PCDatabaseSO.PCDataSOs[pcSaveData.PCID].Stats.StatList.Add(new Stat(tuple.Item1, tuple.Item2));
			}

			// Equipment
			PCDatabaseSO.PCDataSOs[pcSaveData.PCID].Equipment.EquipmentItems.Clear();
			foreach (int equipmentID in pcSaveData.EquipmentIDs)
            {
				PCDatabaseSO.PCDataSOs[pcSaveData.PCID].PCController.EquipmentManager.Equip((SOEquipmentItem)ItemDatabaseSO.Items[equipmentID]);
            }

			// Add SOPCData to HomePCs list from database, based off id from PCSaveData. 
			HomePCs.Add(PCDatabaseSO.PCDataSOs[pcSaveData.PCID]);
        }

		// Load Scavenging PCs list. 
		ScavengingPCs.Clear();
		foreach (int index in gameData.ScavengingPCIDs)
        {
			ScavengingPCs.Add(PCDatabaseSO.PCDataSOs[index]);
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