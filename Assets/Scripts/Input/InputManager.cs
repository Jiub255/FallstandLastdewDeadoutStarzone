using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Have this be a singleton? Or have it created in ApplicationManager? <br/>
/// Singleton might make more sense. It will keep the InputAction subscriptions in the classes that use them, instead
/// of them all being in InputManager. One singleton for input seems fine, lots of classes use it. <br/>
/// TODO - Maybe refactor without singleton later if it causes problems. 
/// </summary>
public class InputManager
{
    /// <summary>
    /// Only called if mouse hasn't moved more than <c>_mouseMovementThreshold</c> between pressing and releasing button. 
    /// </summary>
    public event Action<InputAction.CallbackContext> OnDeselectOrCancel;

    public PlayerControls PC { get; private set; }

    private Vector2 _startingMousePosition;

    [SerializeField]
    private float _mouseMovementThreshold = 0.1f;
    private float _mouseMovementThresholdSquared { get { return _mouseMovementThreshold * _mouseMovementThreshold; } }

    private EventSystem EventSystem { get; set; }
    public bool PointerOverUI { get; private set; }

    public InputManager()
    {
        PC = new PlayerControls();

        EventSystem = EventSystem.current;
        PointerOverUI = EventSystem.IsPointerOverGameObject();

        PC.Camera.RightClick.started += GetStartingMousePosition;
        PC.Camera.RightClick.canceled += HandleRightClick;
    }

    public void OnDisable()
    {
        PC.Camera.RightClick.started -= GetStartingMousePosition;
        PC.Camera.RightClick.canceled -= HandleRightClick;
    }

    public void Update()
    {
        PointerOverUI = EventSystem.IsPointerOverGameObject();
    }

    private void GetStartingMousePosition(InputAction.CallbackContext context)
    {
        _startingMousePosition = PC.Camera.MousePosition.ReadValue<Vector2>();
    }

    private void HandleRightClick(InputAction.CallbackContext context)
    {
        if ((PC.Camera.MousePosition.ReadValue<Vector2>() - _startingMousePosition).sqrMagnitude < _mouseMovementThresholdSquared)
        {
            OnDeselectOrCancel?.Invoke(context);
        }
    }

    // Called by game state machine when changing states. 
    public void EnableStateActionMaps(GameState gameState)
    {
        // Calls one of the below methods depending on which subclass of GameState called this method. 
        EnableStatesActionMaps(gameState as dynamic);
    }

    // Might not use Pause state, just have the different menu states?
    // Or use this state for main menu/submenus (options, save, load, new, etc)? Only navigate main menu using mouse click on buttons? 
    private void EnableStatesActionMaps(GamePauseState pauseState)
    {
        PC.Disable();
    }

    // Used at home base (not during combat/raids/invasions).
    private void EnableStatesActionMaps(GameHomeState homeState)
    {
        PC.Disable();
        PC.Camera.Enable();
//        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }

    // Used in non-combat menus. 
    private void EnableStatesActionMaps(GameHomeMenusState homeMenusState)
    {
        PC.Disable();
//        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }

    // Used in scavenging scenes and home base combat. 
    private void EnableStatesActionMaps(GameCombatState combatState)
    {
        PC.Disable();
        PC.Camera.Enable();
//        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
    }

    // Used in combat menus. 
    private void EnableStatesActionMaps(GameCombatMenusState combatMenusState)
    {
        PC.Disable();
//        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
    }

    // Used in build mode in home base. 
    private void EnableStatesActionMaps(GameBuildState buildState)
    {
        PC.Disable();
        PC.Camera.Enable();
//        PC.Quit.Enable();
        PC.Build.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }
}