using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOListSOPC", fileName = "New SOPC List SO")]
public class SOListSOPC : ScriptableObject
{
	public event Action OnSelectedPCChanged;
	public event Action OnCurrentMenuPCChanged;
	public event Action OnHomeSOPCListChanged;
	public event Action OnScavengingSOPCListChanged;

	[SerializeField]
	private List<SOPC> _homeSOPCSList = new();
	[SerializeField]
	private List<SOPC> _scavengingPCSList = new();
	[SerializeField]
	private GameObject _selectedPC;
	[SerializeField]
	private SOPC _currentMenuSOPC;

	public List<SOPC> HomeSOPCSList { get { return _homeSOPCSList; } }
	public List<SOPC> ScavengingPCSList { get { return _scavengingPCSList; } }
	public GameObject SelectedPC 
	{
		get { return _selectedPC; } 
		set 
		{
			_selectedPC = value;
			OnSelectedPCChanged?.Invoke();
		}
	}
	public SOPC CurrentMenuSOPC 
	{ 
		get { return _currentMenuSOPC; } 
		set
		{		
			_currentMenuSOPC = value;
			OnCurrentMenuPCChanged?.Invoke();
		} 
	}

	public void AddPCToHomeList(SOPC sopc)
    {
		HomeSOPCSList.Add(sopc);
		OnHomeSOPCListChanged?.Invoke();
    }

	public void RemovePCFromHomeList(SOPC sopc)
    {
		if (HomeSOPCSList.Contains(sopc))
        {
			HomeSOPCSList.Remove(sopc);
			OnHomeSOPCListChanged?.Invoke();
        }
        else
        {
			Debug.LogWarning($"{sopc.PCPrefab.name} not on Home SOPC list. ");
        }
    }

	public void AddPCToScavengingList(SOPC sopc)
    {
		ScavengingPCSList.Add(sopc);
		OnScavengingSOPCListChanged?.Invoke();
    }

	public void RemovePCFromScavengingList(SOPC sopc)
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