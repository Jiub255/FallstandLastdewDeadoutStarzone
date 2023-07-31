using System.Collections.Generic;
using UnityEngine;

// Will this be necessary with new PCController class? No, make a PCManager class though, and put these event subscriptions there. 
/// <summary>
/// TODO - Make this class in PCManager, and use it to subscribe to all the usable item effect events. <br/>
/// Could pass in the PCDict and have this class deal with everything. Help declutter PCManager too. 
/// </summary>
public class PCItemUseManager
{
    private Dictionary<SOPCData, PCController> PCControllerDict { get; }
    private SOPCData CurrentMenuPC { get; }

    public PCItemUseManager(Dictionary<SOPCData, PCController> pcControllerDict, SOPCData currentMenuPC)
    {
        PCControllerDict = pcControllerDict;
        CurrentMenuPC = currentMenuPC;

        // Equipment 
        SOEquipmentItem.OnEquip += HandleEquip;
        SOEquipmentItem.OnUnequip += HandleUnequip;

        // Usable items 
        SORelievePain.OnRelievePainEffect += HandleRelievePainEffect;
    }

    public void OnDisable()
    {
        // Equipment 
        SOEquipmentItem.OnEquip -= HandleEquip;
        SOEquipmentItem.OnUnequip -= HandleUnequip;

        // Usable items 
        SORelievePain.OnRelievePainEffect -= HandleRelievePainEffect;
    }

    private void HandleUnequip(SOEquipmentItem item)
    {
        if (CurrentMenuPC != null)
            PCControllerDict[CurrentMenuPC].EquipmentManager.Unequip(item);
        else
            Debug.LogWarning("CurrentMenuPC is null in PCManager. This should never happen, should get set on scene load. ");
    }

    private void HandleEquip(SOEquipmentItem item)
    {
        if (CurrentMenuPC != null)
            PCControllerDict[CurrentMenuPC].EquipmentManager.Equip(item);
        else
            Debug.LogWarning("CurrentMenuPC is null in PCManager. This should never happen, should get set on scene load. ");
    }

    private void HandleRelievePainEffect(int amount, float duration)
    {
        if (CurrentMenuPC != null)
            PCControllerDict[CurrentMenuPC].PainInjuryManager.TemporarilyRelievePain(amount, duration);
        else
            Debug.LogWarning("CurrentMenuPC is null in PCManager. This should never happen, should get set on scene load. ");
    }

    /*	[SerializeField]
        private SOTeamData _currentTeamSO;

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
        }*/
}