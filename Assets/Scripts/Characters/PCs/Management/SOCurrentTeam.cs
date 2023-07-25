using System;
using System.Collections.Generic;
using UnityEngine;

// TODO - Use this as a PCManager, and recieve Usable/Equipment item events and send them to the correct PC? 
// OR, have a separate MonoBehavoiur PCManager, and do that there, just reference this SO? 
/// <summary>
/// Holds a home SOPCDatas list, a scavenging one, a selected PC instance GameObject, and a current menu SOPCData. 
/// </summary>
[CreateAssetMenu(menuName = "Player Characters/SOCurrentTeam", fileName = "Current Team SO")]
public class SOCurrentTeam : ScriptableObject
{
/*	public event Action OnSelectedPCChanged;
	public event Action OnCurrentMenuPCChanged;*/
	public event Action OnHomeSOPCListChanged;
	public event Action OnScavengingSOPCListChanged;

	[SerializeField]
	private List<SOPCData> _homeSOPCSList = new();
	[SerializeField]
	private List<SOPCData> _scavengingPCSList = new();
/*	[SerializeField]
	private GameObject _selectedPC;
	[SerializeField]
	private SOPCData _currentMenuSOPC;*/
	private List<SORecipe> _possibleRecipes = new();

	public List<SOPCData> HomeSOPCSList { get { return _homeSOPCSList; } }
	public List<SOPCData> ScavengingPCSList { get { return _scavengingPCSList; } }
	public List <SORecipe> PossibleRecipes { get { return _possibleRecipes; } set { _possibleRecipes = value; } }

	/// <summary>
	/// Could just have selected bool on SOPCData and send event whenever it changes to true?
	/// Need selected bool for PCManager state runner, <br/>
	/// but need SelectedPC property for 
	/// </summary>
/*	public GameObject SelectedPC 
	{
		get { return _selectedPC; } 
		set 
		{
			_selectedPC = value;
			OnSelectedPCChanged?.Invoke();
		}
	}
	public SOPCData CurrentMenuSOPC 
	{ 
		get { return _currentMenuSOPC; } 
		set
		{		
			_currentMenuSOPC = value;
			OnCurrentMenuPCChanged?.Invoke();
		} 
	}*/

	public SOPCData this[SOPCData pcDataSO]
    {
        get
        {
			foreach (SOPCData homePCDataSO in _homeSOPCSList)
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
		HomeSOPCSList.Add(sopc);

		// PCItemUseManager listens, updates dictionary. 
		OnHomeSOPCListChanged?.Invoke();
    }

	public void RemovePCFromHomeList(SOPCData sopc)
    {
		if (HomeSOPCSList.Contains(sopc))
        {
			HomeSOPCSList.Remove(sopc);

			// PCItemUseManager listens, updates dictionary. 
			OnHomeSOPCListChanged?.Invoke();
        }
        else
        {
			Debug.LogWarning($"{sopc.PCPrefab.name} not on Home SOPC list. ");
        }
    }

	public void AddPCToScavengingList(SOPCData sopc)
    {
		ScavengingPCSList.Add(sopc);
		OnScavengingSOPCListChanged?.Invoke();
    }

	public void RemovePCFromScavengingList(SOPCData sopc)
    {
		if (ScavengingPCSList.Contains(sopc))
        {
			ScavengingPCSList.Remove(sopc);
			OnScavengingSOPCListChanged?.Invoke();
        }
        else
        {
			Debug.LogWarning($"{sopc.PCPrefab.name} not on Scavenging SOPC list. ");
        }
    }

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
			foreach (SOPCData pcSO in HomeSOPCSList)
			{
				totalMedicalSkill += pcSO.Stats[StatType.Medical].ModdedValue;
			}
			return totalMedicalSkill / 1000f;
		}
	}
}