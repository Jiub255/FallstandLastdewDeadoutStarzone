using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// TODO - Have this be a centralized place to control which character is selected. It's all over the place right now. 
// In PCStateMachine, PCSelector, Transparentizer, probably other places. 
// Do like SOListSOPC with events for every change? Or just have scripts that need it reference this? Events seems better. 

/// <summary>
/// Creates and controls a dictionary of PCControllers, each of which has a PCStateMachine, PCStatManager, EquipmentManager, and PainInjuryManager. <br/>
/// Handles item and equipment events, calls the Use or Equip method on selected menu PC. 
/// </summary>
public class PCManager
{
    /// <summary>
    /// TODO - Can this be passed to PCItemUseManager and keep the same references as it gets changed (set) in here?
    /// Pretty sure the reference the same as here if this one gets altered, but what if it gets re-set to another object? 
    /// I think it works fine, classes are reference types, so it should just be passing the reference to the actual CurrentTeamSO, even if it gets changed. 
    /// </summary>
	private SOCurrentTeam CurrentTeamSO { get; }
    private SOPCData CurrentlySelectedPC { get; set; }
    private SOPCData CurrentMenuPC { get; set; }
    private Dictionary<SOPCData, PCController> PCControllerDict { get; } = new();
    private PCSelector PCSelector { get; }
    private PCItemUseManager PCItemUseManager { get; }
    private EventSystem EventSystem { get; }
    private bool PointerOverUI { get; set; }

    public PCManager(SOCurrentTeam currentTeamSO)
    {
        CurrentTeamSO = currentTeamSO;

        PCSelector = new(this, CurrentTeamSO);
        PCItemUseManager = new(PCControllerDict, CurrentMenuPC);
        EventSystem = EventSystem.current;
        PointerOverUI = EventSystem.IsPointerOverGameObject();

        // Set menu PC to first on list to start, so it's never null.
        CurrentMenuPC = CurrentTeamSO.HomeSOPCSList[0];

        PCSelector.OnSelectedNewPC += (pcDataSO) => CurrentlySelectedPC = pcDataSO;
        PCSelector.OnSelectedNewPC += (pcDataSO) => CurrentMenuPC = pcDataSO;
        UICharacter.OnMenuPCChanged += (pcDataSO) => CurrentMenuPC = pcDataSO;

        // Equipment 
        SOEquipmentItem.OnEquip += HandleEquip;
        SOEquipmentItem.OnUnequip += HandleUnequip;

        // TODO - Put these on PCItemUseManager? Kind of clutters up this class. 
        // Usable items 
        SORelievePain.OnRelievePainEffect += HandleRelievePainEffect;

        S.I.IM.PC.World.SelectOrCenter.canceled += HandleClick;
    }

    public void OnDisable()
    {
        PCSelector.OnSelectedNewPC -= (pcDataSO) => CurrentlySelectedPC = pcDataSO;
        PCSelector.OnSelectedNewPC -= (pcDataSO) => CurrentMenuPC = pcDataSO;
        UICharacter.OnMenuPCChanged -= (pcDataSO) => CurrentMenuPC = pcDataSO;

        // TODO - Put these in non-anonymous methods? For readability? 
        // Equipment 
        SOEquipmentItem.OnEquip -= HandleEquip;
        SOEquipmentItem.OnUnequip -= HandleUnequip;

        // Usable items 
        SORelievePain.OnRelievePainEffect -= HandleRelievePainEffect;

        S.I.IM.PC.World.SelectOrCenter.canceled -= HandleClick;

        // Run OnDisable in created class instances. 
        foreach (PCController pcController in PCControllerDict.Values)
        {
            pcController.OnDisable();
        }
        PCSelector.OnDisable();
        PCItemUseManager.OnDisable();
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

    public void UpdateStates()
    {
        PointerOverUI = EventSystem.IsPointerOverGameObject();

        foreach (SOPCData pcDataSO in CurrentTeamSO.HomeSOPCSList)
        {
            pcDataSO.ActiveState.Update(pcDataSO.Selected);
        }
    }

    public void FixedUpdateStates()
    {
        foreach (SOPCData pcDataSO in CurrentTeamSO.HomeSOPCSList)
        {
            pcDataSO.ActiveState.FixedUpdate(pcDataSO.Selected);
        }
    }

    /// <summary>
    /// If no PCs were clicked, and there is a currently selected PC, then run selected PC's HandleClick. <br/>
    /// If a PC was clicked, then run PCSelector's HandleClick and don't even try to run selected PC's HandleClick. 
    /// </summary>
    /// <param name="context"></param>
    private void HandleClick(InputAction.CallbackContext context)
    {
        if (!PointerOverUI)
        {
            if (!PCSelector.CheckIfPCClicked() && CurrentlySelectedPC != null)
            {
                PCControllerDict[CurrentlySelectedPC].PCStateMachine.HandleClick();
            }
        }
    }

    /// <summary>
    /// Called by GameManager after instantiating PCs. 
    /// </summary>
    public void PopulateDictionary()
    {
        PCControllerDict.Clear();

        foreach (SOPCData pcDataSO in CurrentTeamSO.HomeSOPCSList)
        {
            PCControllerDict.Add(pcDataSO, new PCController(pcDataSO, CurrentTeamSO));
        }
    }
}