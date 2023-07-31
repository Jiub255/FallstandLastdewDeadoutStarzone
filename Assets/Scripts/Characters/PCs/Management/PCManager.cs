using System.Collections.Generic;
using System.Linq;
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
    private SOPCData _currentMenuPC;

    /// <summary>
    /// TODO - Can this be passed to PCItemUseManager and keep the same references as it gets changed (set) in here?
    /// Pretty sure the reference the same as here if this one gets altered, but what if it gets re-set to another object? 
    /// I think it works fine, classes are reference types, so it should just be passing the reference to the actual CurrentTeamSO, even if it gets changed. 
    /// </summary>
	private SOTeamData TeamDataSO { get; }
    private SOPCData CurrentlySelectedPC { get; set; }
    /// <summary>
    /// TODO - Put this on TeamDataSO instead so PCSlot can reference it. 
    /// </summary>
//    private Dictionary<SOPCData, PCController> PCControllerDict { get; } = new();
    private PCSelector PCSelector { get; set; }
    private PCItemUseManager PCItemUseManager { get; }
    private InputManager InputManager { get; set; }

    public PCManager(SOTeamData teamDataSO)
    {
        TeamDataSO = teamDataSO;

        // TODO - Will this work? Passing the reference to SOPCData here? What if it gets changed to refer to another SOPCData?
        // Will PCItemUseManager keep the reference to the old one? 
        PCItemUseManager = new(/*TeamDataSO.PCControllerDict, */ref _currentMenuPC);

        InputManager = S.I.IM;

        // Set menu PC to first on list to start, so it's never null.
        _currentMenuPC = TeamDataSO.HomePCs[0];

        PCSelector.OnSelectedNewPC += (pcDataSO) => CurrentlySelectedPC = pcDataSO;
        PCSelector.OnSelectedNewPC += (pcDataSO) => _currentMenuPC = pcDataSO;
        UICharacter.OnMenuPCChanged += (pcDataSO) => _currentMenuPC = pcDataSO;
        SpawnPoint.OnSceneStart += InitializeScene;

        S.I.IM.PC.World.SelectOrCenter.canceled += HandleClick;
    }

    public void OnDisable()
    {
        PCSelector.OnSelectedNewPC -= (pcDataSO) => CurrentlySelectedPC = pcDataSO;
        PCSelector.OnSelectedNewPC -= (pcDataSO) => _currentMenuPC = pcDataSO;
        UICharacter.OnMenuPCChanged -= (pcDataSO) => _currentMenuPC = pcDataSO;
        SpawnPoint.OnSceneStart -= InitializeScene;

        S.I.IM.PC.World.SelectOrCenter.canceled -= HandleClick;

        // Run OnDisable in created class instances. 
//        foreach (PCController pcController in TeamDataSO.PCControllerDict.Values)
        foreach (PCController pcController in TeamDataSO.HomePCs.Select(pcDataSO => pcDataSO.PCController))
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

                TeamDataSO.HomePCs[i].PCController = new PCController(TeamDataSO.HomePCs[i], TeamDataSO);
            }

//            PopulateDictionary();

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
//                TeamDataSO.PCControllerDict[CurrentlySelectedPC].PCStateMachine.HandleClick();
                CurrentlySelectedPC.PCController.PCStateMachine.HandleClick();
            }
        }
    }

    /// <summary>
    /// Called after instantiating PCs. 
    /// </summary>
/*    private void PopulateDictionary()
    {
        TeamDataSO.PCControllerDict.Clear();

        foreach (SOPCData pcDataSO in TeamDataSO.HomePCs)
        {
//            TeamDataSO.PCControllerDict.Add(pcDataSO, new PCController(pcDataSO, TeamDataSO));
            pcDataSO.PCController = new PCController(pcDataSO, TeamDataSO);
        }
    }*/
}