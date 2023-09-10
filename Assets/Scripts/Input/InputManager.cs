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

    /// <summary>
    /// Just for checking if mouse is over UI. Not sure if this will work in build. 
    /// </summary>
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

    /// <summary>
    /// Called by game state machine when changing states. 
    /// </summary>
    /// <param name="gameState"></param>
    public void EnableStateActionMaps(GameState gameState)
    {
        // Calls one of the below methods depending on which subclass of GameState called this method. 
        EnableStatesActionMaps(gameState as dynamic);
    }

    // Might not use Pause state, just have the different menu states?
    // Or use this state for main menu/submenus (options, save, load, new, etc)? Only navigate main menu using mouse click on buttons? 
/*    private void EnableStatesActionMaps(GamePauseState pauseState)
    {
        PC.Disable();
    }*/

    // Used at home base (not during combat/raids/invasions).
    private void EnableStatesActionMaps(GameHomeState homeState)
    {
        if (!PC.Camera.enabled) PC.Camera.Enable();
        if (!PC.World.enabled) PC.World.Enable();
        if (!PC.InventoryMenu.enabled) PC.InventoryMenu.Enable();
        if (!PC.NonCombatMenus.enabled) PC.NonCombatMenus.Enable();
        
        if (PC.Build.enabled) PC.Build.Disable();
    }

    // Used in non-combat menus. 
    private void EnableStatesActionMaps(GameHomeMenusState homeMenusState)
    {
        if (!PC.InventoryMenu.enabled) PC.InventoryMenu.Enable();
        if (!PC.NonCombatMenus.enabled) PC.NonCombatMenus.Enable();

        if (PC.Camera.enabled) PC.Camera.Disable();
        if (PC.World.enabled) PC.World.Disable();
        if (PC.Build.enabled) PC.Build.Disable();
    }

    // Used in scavenging scenes and home base combat. 
    private void EnableStatesActionMaps(GameCombatState combatState)
    {
        if (!PC.Camera.enabled) PC.Camera.Enable();
        if (!PC.World.enabled) PC.World.Enable();
        if (!PC.InventoryMenu.enabled) PC.InventoryMenu.Enable();

        if (PC.Build.enabled) PC.Build.Disable();
        if (PC.NonCombatMenus.enabled) PC.NonCombatMenus.Disable();
    }

    // Used in combat menus. 
    private void EnableStatesActionMaps(GameCombatMenusState combatMenusState)
    {
        if (!PC.InventoryMenu.enabled) PC.InventoryMenu.Enable();

        if (PC.Camera.enabled) PC.Camera.Disable();
        if (PC.World.enabled) PC.World.Disable();
        if (PC.Build.enabled) PC.Build.Disable();
        if (PC.NonCombatMenus.enabled) PC.NonCombatMenus.Disable();
    }

    // Used in build mode in home base. 
    private void EnableStatesActionMaps(GameBuildState buildState)
    {
        if (!PC.Camera.enabled) PC.Camera.Enable();
        if (!PC.Build.enabled) PC.Build.Enable();
        if (!PC.InventoryMenu.enabled) PC.InventoryMenu.Enable();
        if (!PC.NonCombatMenus.enabled) PC.NonCombatMenus.Enable();

        if (PC.World.enabled) PC.World.Disable();
    }
}