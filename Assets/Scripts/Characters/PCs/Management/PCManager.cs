using System.Collections.Generic;
using UnityEngine;
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
	private SOTeamData TeamDataSO { get; }
    private SOPCData CurrentlySelectedPC { get; set; }
    private SOPCData CurrentMenuPC { get; set; }
    private Dictionary<SOPCData, PCController> PCControllerDict { get; } = new();
    private PCSelector PCSelector { get; set; }
    private PCItemUseManager PCItemUseManager { get; }
    private InputManager InputManager { get; set; }

    public PCManager(SOTeamData teamDataSO)
    {
        TeamDataSO = teamDataSO;

        PCItemUseManager = new(PCControllerDict, CurrentMenuPC);

        InputManager = S.I.IM;

        // Set menu PC to first on list to start, so it's never null.
        CurrentMenuPC = TeamDataSO.HomePCs[0];

        PCSelector.OnSelectedNewPC += (pcDataSO) => CurrentlySelectedPC = pcDataSO;
        PCSelector.OnSelectedNewPC += (pcDataSO) => CurrentMenuPC = pcDataSO;
        UICharacter.OnMenuPCChanged += (pcDataSO) => CurrentMenuPC = pcDataSO;
        SpawnPoint.OnSceneStart += InitializeScene;

        S.I.IM.PC.World.SelectOrCenter.canceled += HandleClick;
    }

    public void OnDisable()
    {
        PCSelector.OnSelectedNewPC -= (pcDataSO) => CurrentlySelectedPC = pcDataSO;
        PCSelector.OnSelectedNewPC -= (pcDataSO) => CurrentMenuPC = pcDataSO;
        UICharacter.OnMenuPCChanged -= (pcDataSO) => CurrentMenuPC = pcDataSO;
        SpawnPoint.OnSceneStart -= InitializeScene;

        S.I.IM.PC.World.SelectOrCenter.canceled -= HandleClick;

        // Run OnDisable in created class instances. 
        foreach (PCController pcController in PCControllerDict.Values)
        {
            pcController.OnDisable();
        }
        PCSelector.OnDisable();
        PCItemUseManager.OnDisable();
    }

    private void InitializeScene(Vector3 spawnPosition)
    {
        InstantiatePCs(spawnPosition);
    }

    private void InstantiatePCs(Vector3 spawnPosition)
    {
        if (TeamDataSO.HomePCs.Count > 0)
        {
            for (int i = 0; i < TeamDataSO.HomePCs.Count; i++)
            {
                // Will UnityEngine.Object.Instantiate work? Or should this be done in GameManager? 
                TeamDataSO.HomePCs[i].PCInstance = Object.Instantiate(
                    TeamDataSO.HomePCs[i].PCPrefab,
                    new Vector3(3 * i, 0f, 0f) + spawnPosition,
                    Quaternion.identity);
            }

            PopulateDictionary();

            // This has to be constructed after PCs have been instantiated. 
            PCSelector = new(TeamDataSO);
        }
        else
        {
            Debug.LogWarning("No PCs on HomeSOPCSList in CurrentTeamSO. Can't play the game without PCs. ");
        }
    }

    public void UpdateStates()
    {
        foreach (SOPCData pcDataSO in TeamDataSO.HomePCs)
        {
            pcDataSO.ActiveState.Update(pcDataSO.Selected);
        }
    }

    public void FixedUpdateStates()
    {
        foreach (SOPCData pcDataSO in TeamDataSO.HomePCs)
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
        if (!InputManager.PointerOverUI)
        {
            if (!PCSelector.CheckIfPCClicked() && CurrentlySelectedPC != null)
            {
                PCControllerDict[CurrentlySelectedPC].PCStateMachine.HandleClick();
            }
        }
    }

    /// <summary>
    /// Called after instantiating PCs. 
    /// </summary>
    private void PopulateDictionary()
    {
        PCControllerDict.Clear();

        foreach (SOPCData pcDataSO in TeamDataSO.HomePCs)
        {
            PCControllerDict.Add(pcDataSO, new PCController(pcDataSO, TeamDataSO));
        }
    }
}