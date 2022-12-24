using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Will this work? Can I have all the (public static) InputActions here too?
    // Seems like it will work

    // How will action maps work?

    public static PlayerInput playerInput;

    // Gameplay Action Map
    public /*static*/ InputAction mousePositionAction;
    public /*static*/ InputAction moveCameraAction;
    public /*static*/ InputAction rotateCameraAction;
    public /*static*/ InputAction mouseDeltaAction;
    public /*static*/ InputAction mouseWheelAction;
    public /*static*/ InputAction mouseLeftAction;
    public /*static*/ InputAction mouseRightAction;
    public /*static*/ InputAction openInventoryAction;

    // UI Action Map

    public /*static*/ InputAction closeInventoryAction;


    // Build Action Map



    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Gameplay");

        // Gameplay Action Map
        mousePositionAction = playerInput.actions["MousePosition"];
        moveCameraAction = playerInput.actions["MoveCamera"];
        rotateCameraAction = playerInput.actions["RotateCamera"];
        mouseDeltaAction = playerInput.actions["MouseDelta"];
        mouseWheelAction = playerInput.actions["MouseWheel"];
        mouseLeftAction = playerInput.actions["MouseLeft"];
        mouseRightAction = playerInput.actions["MouseRight"];
        openInventoryAction = playerInput.actions["OpenInventory"];

        // UI Action Map
        closeInventoryAction = playerInput.actions["CloseInventory"];

        // Build Action Map

    }

    public /*static*/ void ChangeActionMap(InputActionMap newActionMap)
    {
        playerInput.SwitchCurrentActionMap(newActionMap.name);

        Debug.Log(playerInput.currentActionMap.name);
    }

    public /*static*/ void ChangeActionMap(string actionMapName)
    {
        playerInput.SwitchCurrentActionMap(actionMapName);

        Debug.Log(playerInput.currentActionMap.name);
    }

    /*    [SerializeField]
        private InputActionAsset playerControlsCopy;
        public static InputActionAsset playerControls;

        [SerializeField]
        private InputActionMap defaultActionMap;
        public static InputActionMap currentActionMap;

        private void Awake()
        {
            playerControls = playerControlsCopy;
            Debug.Log("Player controls: " + playerControls.name);
            currentActionMap = defaultActionMap;
            Debug.Log("Action map: " + currentActionMap.name);
        }

        public static void ChangeActionMap(InputActionMap newActionMap)
        {
            newActionMap.Enable();
        }*/
}