using System.Collections.Generic;
using UnityEngine;

public class PCItemUseManager : MonoBehaviour
{
	[SerializeField]
	private SOListSOPC _currentTeamSO;

    // TODO - Will checking for home/scavenging be necessary? 
    // If just using the list of all PCs and the CurrentMenuPC, then it shouldn't be a problem as long as the 
    // CurrentMenuPC gets changed to someone on the scavenging team when going scavenging. 
/*    // True if at home, false if scavenging. 
    public bool Home = true; */

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

	private Dictionary<int, PCManagers> _idsAndManagers = new();

    // Subscribe to all Usable and Equipment item events here. 
    private void Start()
    {
        PopulateDictionary();

        // Equipment 
        SOEquipmentItem.OnEquip += (item) =>
        {
            _idsAndManagers[_currentTeamSO.CurrentMenuSOPC.GetInstanceID()].EquipmentManager.Equip(item);
        };
        SOEquipmentItem.OnUnequip += (item) =>
        {
            _idsAndManagers[_currentTeamSO.CurrentMenuSOPC.GetInstanceID()].EquipmentManager.Unequip(item);
        };

        // Usable items 
        SORelievePain.OnRelievePainEffect += (amount, duration) => 
        { 
            _idsAndManagers[_currentTeamSO.CurrentMenuSOPC.GetInstanceID()].PainInjuryManager.TemporarilyRelievePain(amount, duration); 
        };

        _currentTeamSO.OnHomeSOPCListChanged += PopulateDictionary;
    }

    private void OnDisable()
    {
        // Equipment 
        SOEquipmentItem.OnEquip -= (item) =>
        {
            _idsAndManagers[_currentTeamSO.CurrentMenuSOPC.GetInstanceID()].EquipmentManager.Equip(item);
        };
        SOEquipmentItem.OnUnequip -= (item) =>
        {
            _idsAndManagers[_currentTeamSO.CurrentMenuSOPC.GetInstanceID()].EquipmentManager.Unequip(item);
        };

        // Usable items 
        SORelievePain.OnRelievePainEffect -= (amount, duration) => 
        { 
            _idsAndManagers[_currentTeamSO.CurrentMenuSOPC.GetInstanceID()].PainInjuryManager.TemporarilyRelievePain(amount, duration);
        };

        _currentTeamSO.OnHomeSOPCListChanged += PopulateDictionary;
    }

    // Call this on Awake and whenever the list changes. 
    private void PopulateDictionary()
    {
        foreach (SOPCData pcDataSO in _currentTeamSO.HomeSOPCSList)
        {
            if (pcDataSO.PCInstance != null)
            {
                PainInjuryManager painInjuryManager = pcDataSO.PCInstance.GetComponentInChildren<PainInjuryManager>();
                PCStatManager pcStatManager = pcDataSO.PCInstance.GetComponentInChildren<PCStatManager>();
                EquipmentManager equipmentManager = pcDataSO.PCInstance.GetComponentInChildren<EquipmentManager>();

                _idsAndManagers.Add(pcDataSO.PCInstance.GetInstanceID(), new PCManagers(painInjuryManager, pcStatManager, equipmentManager));
            }
            else
            {
                Debug.Log($"No PCInstance found on {pcDataSO.name}");
            }
        }
    }
}