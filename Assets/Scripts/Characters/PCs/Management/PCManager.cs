using System;
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
    /// <summary>
    /// UIPCHUD and UICharacter listen, set up slots. 
    /// </summary>
    public static event Action OnAfterPCsInstantiated;

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
    private GameManager GameManager { get; set; }
//    private Vector3 SpawnPosition { get; set; }

    /// <summary>
    /// TODO - Delay calling this until new/load/continue game is chosen from menu and data is loaded? <br/>
    /// Similarly for inventory, building managers. Any in-game, save-dependent stuff really. 
    /// </summary>
    public PCManager(SOTeamData teamDataSO, InputManager inputManager, GameManager gameManager)
    {
        TeamDataSO = teamDataSO;
        InputManager = inputManager;
        GameManager = gameManager;

        // TODO - Will this work? Passing the reference to SOPCData here? What if it gets changed to refer to another SOPCData?
        // Will PCItemUseManager keep the reference to the old one? 
        // Use PCDatabase instead? Probably, then just store index/ID. 
        PCItemUseManager = new(/*TeamDataSO.PCControllerDict, */ref _currentMenuPC);

        // Set menu PC to first on list to start, so it's never null.
        _currentMenuPC = TeamDataSO.HomePCs[0];

        PCSelector.OnSelectedNewPC += (pcDataSO) => CurrentlySelectedPC = pcDataSO;
        PCSelector.OnSelectedNewPC += (pcDataSO) => _currentMenuPC = pcDataSO ? pcDataSO : _currentMenuPC;
        UICharacter.OnMenuPCChanged += (pcDataSO) => _currentMenuPC = pcDataSO;
        SpawnPoint.OnSceneStart += InitializeScene;

        inputManager.PC.World.SelectOrCenter.canceled += HandleClick;
    }

    public void SaveData(GameSaveData gameData)
    {
        // Get PC data from SOTeamData. 
        TeamDataSO.SaveData(gameData);
    }

    public void LoadData(GameSaveData gameData)
    {
        // Load PC data onto SOTeamData. 
        TeamDataSO.LoadData(gameData);

        // How to do this? Make sure it happens after scene load call? Cache spawn point? 
        // OR, just have LoadData load the data, and wait and let SpawnPoint do the instantiate event. 
//        InstantiatePCs(SpawnPosition);
    }

    public void OnDisable()
    {
        PCSelector.OnSelectedNewPC -= (pcDataSO) => CurrentlySelectedPC = pcDataSO;
        PCSelector.OnSelectedNewPC -= (pcDataSO) => _currentMenuPC = pcDataSO ? pcDataSO : _currentMenuPC;
        UICharacter.OnMenuPCChanged -= (pcDataSO) => _currentMenuPC = pcDataSO;
        SpawnPoint.OnSceneStart -= InitializeScene;

        InputManager.PC.World.SelectOrCenter.canceled -= HandleClick;

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
//        SpawnPosition = spawnPosition;
        InstantiatePCs(spawnPosition);
    }

    /// <summary>
    /// TODO - How to make sure PC data gets loaded onto SOs before this gets called? 
    /// </summary>
    private void InstantiatePCs(Vector3 spawnPosition)
    {
//        Debug.Log("Instantiate PCs Called");

        if (TeamDataSO.HomePCs.Count > 0)
        {
            for (int i = 0; i < TeamDataSO.HomePCs.Count; i++)
            {
                // Instantiate PC. 
                GameObject pcInstance = UnityEngine.Object.Instantiate(
                    TeamDataSO.HomePCs[i].PCPrefab,
                    new Vector3(3 * i, 0f, 0f) + spawnPosition,
                    Quaternion.identity);

                // Set SOPCData references for this PC. 
                TeamDataSO.HomePCs[i].PCInstance = pcInstance;
                TeamDataSO.HomePCs[i].SelectedPCIcon = pcInstance.GetComponentInChildren<SelectedPCIcon>();
                TeamDataSO.HomePCs[i].PCController = new PCController(TeamDataSO.HomePCs[i], TeamDataSO, InputManager, GameManager);
            }

            // This has to be constructed after PCs have been instantiated. 
            PCSelector = new(TeamDataSO, InputManager);

            // Changing PC to first on list to set CurrentMenuPC on other scripts, then setting back to null
            // so no PC is world selected, but there is a CurrentMenuPC from the start. 
            PCSelector.ChangePC(TeamDataSO.HomePCs[0].PCInstance);
            PCSelector.ChangePC(null);

            OnAfterPCsInstantiated?.Invoke();
        }
        else
        {
            Debug.LogWarning("No PCs on HomeSOPCSList in CurrentTeamSO. Can't play the game without PCs. ");
        }
    }

    /// <summary>
    /// TODO - Why not handle this from PCStateMachine? <br/>
    /// But that would just add an unnecessary step essentially, since it would still be getting active state from SOPCData. 
    /// </summary>
    public void UpdateStates()
    {
        foreach (SOPCData pcDataSO in TeamDataSO.HomePCs)
        {
            if (pcDataSO.ActiveState != null)
                pcDataSO.ActiveState.Update(pcDataSO.Selected);
//            Debug.Log($"Active state null: {pcDataSO.ActiveState == null}");
        }
    }

    public void FixedUpdateStates()
    {
        foreach (SOPCData pcDataSO in TeamDataSO.HomePCs)
        {
            if (pcDataSO.ActiveState != null)
                pcDataSO.ActiveState.FixedUpdate(pcDataSO.Selected);
        }
    }

    /// <summary>
    /// If no PCs were clicked, and there is a currently selected PC, then run selected PC's HandleClick. <br/>
    /// If a PC was clicked, then run PCSelector's HandleClick and don't even try to run selected PC's HandleClick. 
    /// </summary>
    private void HandleClick(InputAction.CallbackContext context)
    {
//        Debug.Log($"HandleClick called, PointerOverUI: {InputManager.PointerOverUI}");

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