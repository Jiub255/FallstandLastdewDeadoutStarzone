using System.Collections.Generic;
using UnityEngine;

public class PCItemUseManager : MonoBehaviour
{
	[SerializeField]
	private SOListSOPC _currentTeamSO;

	private class PCManagers
    {
		public PainInjuryManager PainInjuryManager { get; }
		public PCStatManager PCStatManager { get; }
        public EquipmentManager EquipmentManager { get; }

        public PCManagers(PainInjuryManager painInjuryManager, PCStatManager pcStatManager, EquipmentManager equipmentManager)
        {
            PainInjuryManager = painInjuryManager;
            PCStatManager = pcStatManager;
            EquipmentManager = equipmentManager;
        }
    }

	private Dictionary<SOPCData, PCManagers> _pcDataSOsAndManagers = new();

    // Subscribe to all Usable and Equipment item events here. 
    private void Start()
    {
        PopulateDictionary();

        // Equipment 
        SOEquipmentItem.OnEquip += (item) =>
        {
            _pcDataSOsAndManagers[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Equip(item);
        };
        SOEquipmentItem.OnUnequip += (item) =>
        {
            _pcDataSOsAndManagers[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Unequip(item);
        };

        // Usable items 
        SORelievePain.OnRelievePainEffect += (amount, duration) => 
        { 
            _pcDataSOsAndManagers[_currentTeamSO.CurrentMenuSOPC].PainInjuryManager.TemporarilyRelievePain(amount, duration); 
        };

        _currentTeamSO.OnHomeSOPCListChanged += PopulateDictionary;
    }

    private void OnDisable()
    {
        // Equipment 
        SOEquipmentItem.OnEquip -= (item) =>
        {
            _pcDataSOsAndManagers[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Equip(item);
        };
        SOEquipmentItem.OnUnequip -= (item) =>
        {
            _pcDataSOsAndManagers[_currentTeamSO.CurrentMenuSOPC].EquipmentManager.Unequip(item);
        };

        // Usable items 
        SORelievePain.OnRelievePainEffect -= (amount, duration) => 
        { 
            _pcDataSOsAndManagers[_currentTeamSO.CurrentMenuSOPC].PainInjuryManager.TemporarilyRelievePain(amount, duration);
        };

        _currentTeamSO.OnHomeSOPCListChanged += PopulateDictionary;
    }

    // Call this on Awake and whenever the list changes. 
    private void PopulateDictionary()
    {
        _pcDataSOsAndManagers.Clear();

        foreach (SOPCData pcDataSO in _currentTeamSO.HomeSOPCSList)
        {
            if (pcDataSO.PCInstance != null)
            {
                PainInjuryManager painInjuryManager = pcDataSO.PCInstance.GetComponentInChildren<PainInjuryManager>();
                PCStatManager pcStatManager = pcDataSO.PCInstance.GetComponentInChildren<PCStatManager>();
                EquipmentManager equipmentManager = pcDataSO.PCInstance.GetComponentInChildren<EquipmentManager>();

                _pcDataSOsAndManagers.Add(pcDataSO, new PCManagers(painInjuryManager, pcStatManager, equipmentManager));
            }
            else
            {
                Debug.Log($"No PCInstance found on {pcDataSO.name}");
            }
        }
    }
}