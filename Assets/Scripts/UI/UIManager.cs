using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Maybe put this on TheSingleton?
public class UIManager : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField]
    private Canvas inventoryCanvas;
    [SerializeField]
    private Canvas buildCanvas;

    [Header("Game State SO's")]
    [SerializeField]
    private GameStateSO buildState;
    [SerializeField]
    private GameStateSO homeState;
    [SerializeField]
    private GameStateSO inventoryState;

    // MenuUIRefresher listens to initialize the UI displays
    public static event Action onOpenedMenu;

    // Not sure about this
    //public List<CanvasState> CSList = new List<CanvasState>();

    public static bool gamePaused = false;

    private void Start()
    {
        S.I.InputManager.openInventoryAction.performed += OpenInventory;
       // S.I.InputManager.openInventoryAction.performed += OpenInventory;
        S.I.InputManager.closeInventoryAction.performed += CloseInventory;
    }

    private void OnDisable()
    {
        S.I.InputManager.openInventoryAction.performed -= OpenInventory;
        S.I.InputManager.closeInventoryAction.performed -= CloseInventory;
    }

    private void OpenBuildMenu(InputAction.CallbackContext context)
    {
        // Open build canvas
        buildCanvas.gameObject.SetActive(true);

        // Change game state and action map
        S.I.GameStateMachine.ChangeStateAndActionMap(buildState);

        // BuildMenuUI listens to initialize display
        onOpenedMenu.Invoke();

        // Pause gameplay if not already paused
        if (!gamePaused)
        {
            PauseGame();
        }
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
        // Open inventory canvas
        inventoryCanvas.gameObject.SetActive(true);

        // Change game state and action map
        S.I.GameStateMachine.ChangeStateAndActionMap(inventoryState);

        // InventoryUI listens to initialize display
        onOpenedMenu.Invoke();

        // Pause gameplay if not already paused
        if (!gamePaused)
        {
            PauseGame();
        }
    }

    private void CloseInventory(InputAction.CallbackContext context)
    {
        inventoryCanvas.gameObject.SetActive(false);

        // TheSingleton.Instance.InputManager.ChangeActionMap("Gameplay");

        S.I.GameStateMachine.ChangeStateAndActionMap(homeState);
       
        if (gamePaused)
        {
            UnpauseGame();
        }
    }

    public static void PauseGame()
    {
        gamePaused = true;

        Time.timeScale = 0f;
    }

    public static void UnpauseGame()
    {
        gamePaused = false;

        Time.timeScale = 1f;
    }
















/*
    private void OpenMenu(InputAction.CallbackContext context)
    {
        CanvasState canvasState = ActionNameToCanvasState(context.action.name);

        // Open Canvas
        canvasState.canvas.gameObject.SetActive(true);

        // Change game state and action map
        S.I.GameStateMachine.ChangeStateAndActionMap(canvasState.gameStateSO);

        // BuildMenuUI listens to initialize display
        onOpenedMenu.Invoke();

        // Pause gameplay if not already paused
        if (!gamePaused)
        {
            PauseGame();
        }
    }*/

    // Needs a lot of work. What if I open inv while in build then close inv? What about build?
    // Should probably plan how the UI's gonna work first
        // Tabs for build, equip, craft, and then inv on the left?
        // Or tabs for equip and craft, inv on left, and build it's own separate mode where you can't access the other menus?
        // Things able to be open together:
        // Not sure yet. Just do Inv and build for now, worry about the rest later.
/*    private void CloseMenu(InputAction.CallbackContext context)
    {
        CanvasState canvasState = ActionNameToCanvasState(context.action.name);
        
        canvasState.canvas.gameObject.SetActive(false);

        S.I.GameStateMachine.ChangeStateAndActionMap(canvasState.gameStateSO);

        if (gamePaused)
        {
            UnpauseGame();
        }
    }

    private CanvasState ActionNameToCanvasState(string actionName)
    {
        switch (actionName)
        {
            case "openInventoryAction":
                return CSList[1];
            case "openBuildMenuAction":
                return CSList[0];
            default: 
                return null;
        }
    }*/
}