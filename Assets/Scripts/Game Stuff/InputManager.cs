using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Will this work? Can I have all the (public static) InputActions here too?
    // Seems like it will work

    // How will action maps work?
    // What if I want some of the same (maybe all) actions on my Build action map too?
    // Just have the extra actions on build, like rotate building and such, and enable the
    // build action map on top of the gameplay one instead of switching to just using it.
    // Maybe have a home action map with all the shared controls, then the additional controls for each 
    // other map. Then switch them out as needed, but always keep the gameplay one enabled.

    // Do I even need the UI action map? Or does the event system prebuilt one handle it?

    public static PlayerInput playerInput;

    // GAMEPLAY ACTION MAP ALWAYS ENABLED
    // Gameplay Action Map
    [HideInInspector]
    public InputAction mousePositionAction;
    [HideInInspector]
    public InputAction mouseDeltaAction;
    [HideInInspector]
    public InputAction moveCameraAction;
    [HideInInspector]
    public InputAction dragCameraAction;
    [HideInInspector]
    public InputAction rotateCameraAction;
    [HideInInspector]
    public InputAction zoomAction;
    [HideInInspector]
    public InputAction selectAction;
    [HideInInspector]
    public InputAction openInventoryAction;
    [HideInInspector]
    public InputAction openBuildMenuAction;
    [HideInInspector]
    public InputAction stopLootingAction;

    // ADDITIONAL ACTION MAPS
    // UI Action Map
    // For Inventory, Crafting menus. Build menu?
    [HideInInspector]
    public InputAction closeInventoryAction;

    // Build Action Map
    [HideInInspector]
    public InputAction rotateBuildingAction;
    [HideInInspector]
    public InputAction closeBuildMenuAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Gameplay");

        // Gameplay Action Map
        mousePositionAction = playerInput.actions["MousePosition"];
        mouseDeltaAction = playerInput.actions["MouseDelta"];
        moveCameraAction = playerInput.actions["MoveCamera"];
        dragCameraAction = playerInput.actions["DragCamera"];
        rotateCameraAction = playerInput.actions["RotateCamera"];
        zoomAction = playerInput.actions["Zoom"];
        selectAction = playerInput.actions["Select"];
        openInventoryAction = playerInput.actions["OpenInventory"];
        openBuildMenuAction = playerInput.actions["OpenBuildMenu"];
        
        // Disable until character in loot mode
        stopLootingAction = playerInput.actions["StopLooting"];
        stopLootingAction.Disable();

        // UI Action Map
        closeInventoryAction = playerInput.actions["CloseInventory"];

        // Build Action Map
        rotateBuildingAction = playerInput.actions["RotateBuilding"];
        closeBuildMenuAction = playerInput.actions["CloseBuildMenu"];
    }

    // Need to be more careful here. Which action maps get enabled/disabled depends
         // on which mode we're entering.
    private void Start()
    {
        // S.I.GameStateMachine.onEnteredState += ChangeMapFromState;
        S.I.GameStateMachine.onChangedState += ChangeMapsFromStates;
    }

    private void OnDisable()
    {
       // S.I.GameStateMachine.onEnteredState -= ChangeMapFromState;
        S.I.GameStateMachine.onChangedState -= ChangeMapsFromStates;
    }

    private void ChangeMapsFromStates(GameStateSO previousState, GameStateSO newState)
    {
        // Could have each GameStateSO carry instructions on what action maps to
        // enable/disable when entering/exiting that state.
        foreach (string actionMapName in previousState.StateActionMaps)
        {
            DisableActionMap(actionMapName);
        }
        foreach (string actionMapName in newState.StateActionMaps)
        {
            EnableActionMap(actionMapName);
        }
    }

    public void EnableActionMap(string actionMapName)
    {
        InputActionMap actionMap = playerInput.actions.FindActionMap(actionMapName);
        if (!actionMap.enabled)
        {
            actionMap.Enable();
        }
    }

    public void DisableActionMap(string actionMapName)
    {
        InputActionMap actionMap = playerInput.actions.FindActionMap(actionMapName);
        if (actionMap.enabled)
        {
            actionMap.Disable();
        }
    }

/*    public void ChangeMapFromState(GameStateSO newState)
    {
        ChangeActionMap(newState.stateActionMapName);
    }

    public void ChangeActionMap(InputActionMap newActionMap)
    {
        playerInput.SwitchCurrentActionMap(newActionMap.name);

        Debug.Log(playerInput.currentActionMap.name);
    }

    public void ChangeActionMap(string actionMapName)
    {
        playerInput.SwitchCurrentActionMap(actionMapName);

        Debug.Log(playerInput.currentActionMap.name);
    }*/
}