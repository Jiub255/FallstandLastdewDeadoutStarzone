using System;
using System.Collections.Generic;
using UnityEngine;

// TODO - Use this as a PCManager, and recieve Usable/Equipment item events and send them to the correct PC? 
// OR, have a separate MonoBehavoiur PCManager, and do that there, just reference this SO? 
[CreateAssetMenu(menuName = "Player Characters/SOListSOPC", fileName = "New SOPC List SO")]
public class SOListSOPC : ScriptableObject
{
	public event Action OnSelectedPCChanged;
	public event Action OnCurrentMenuPCChanged;
	public event Action OnHomeSOPCListChanged;
	public event Action OnScavengingSOPCListChanged;

	[SerializeField]
	private List<SOPCData> _homeSOPCSList = new();
	[SerializeField]
	private List<SOPCData> _scavengingPCSList = new();
	[SerializeField]
	private GameObject _selectedPC;
	[SerializeField]
	private SOPCData _currentMenuSOPC;

	public List<SOPCData> HomeSOPCSList { get { return _homeSOPCSList; } }
	public List<SOPCData> ScavengingPCSList { get { return _scavengingPCSList; } }
	public GameObject SelectedPC 
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
}