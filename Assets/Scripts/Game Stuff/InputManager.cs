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
    public InputAction mousePositionAction;
    public InputAction moveCameraAction;
    public InputAction rotateCameraAction;
    public InputAction mouseDeltaAction;
    public InputAction zoomAction;
    public InputAction selectAction;
    public InputAction dragCameraAction;
    public InputAction openInventoryAction;
    public InputAction openBuildMenuAction;

    // ADDITIONAL ACTION MAPS
    // UI Action Map
    // For Inventory, Crafting menus. Build menu?
    public InputAction closeInventoryAction;

    // Build Action Map
    public InputAction rotateBuildingAction;
    public InputAction closeBuildMenuAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Gameplay");

        // Gameplay Action Map
        mousePositionAction = playerInput.actions["MousePosition"];
        moveCameraAction = playerInput.actions["MoveCamera"];
        rotateCameraAction = playerInput.actions["RotateCamera"];
        mouseDeltaAction = playerInput.actions["MouseDelta"];
        zoomAction = playerInput.actions["Zoom"];
        selectAction = playerInput.actions["Select"];
        dragCameraAction = playerInput.actions["DragCamera"];
        openInventoryAction = playerInput.actions["OpenInventory"];
        openBuildMenuAction = playerInput.actions["OpenBuildMenu"];

        // UI Action Map
        closeInventoryAction = playerInput.actions["CloseInventory"];

        // Build Action Map
        rotateBuildingAction = playerInput.actions["RotateBuilding"];
        closeBuildMenuAction = playerInput.actions["CloseBuildMenu"];
    }

    private void Start()
    {
        S.I.GameStateMachine.onChangedState += ChangeMapFromState;
    }

    private void OnDisable()
    {
        S.I.GameStateMachine.onChangedState -= ChangeMapFromState;
    }

    public void ChangeMapFromState(GameStateSO newState)
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
    }
}