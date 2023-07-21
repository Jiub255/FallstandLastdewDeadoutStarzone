using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext> OnDeselectOrCancel;

    public PlayerControls PC { get; private set; }

    private Vector2 _startingMousePosition;
    [SerializeField]
    private float _mouseMovementThreshold = 0.1f;
    private float _mouseMovementThresholdSquared { get { return _mouseMovementThreshold * _mouseMovementThreshold; } }

    private void Awake()
    {
        PC = new PlayerControls();

        PC.Camera.RightClick.started += GetStartingMousePosition;
        PC.Camera.RightClick.canceled += HandleRightClick;
    }

    private void OnDisable()
    {
        PC.Camera.RightClick.started -= GetStartingMousePosition;
        PC.Camera.RightClick.canceled -= HandleRightClick;
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
        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }

    // Used in non-combat menus. 
    public void EnableStatesActionMaps(GameHomeMenusState homeMenusState)
    {
        PC.Disable();
        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }

    // Used in scavenging scenes and home base combat. 
    public void EnableStatesActionMaps(GameCombatState combatState)
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
    }

    // Used in combat menus. 
    public void EnableStatesActionMaps(GameCombatMenusState combatMenusState)
    {
        PC.Disable();
        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
    }

    // Used in build mode in home base. 
    public void EnableStatesActionMaps(GameBuildState buildState)
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.Build.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }
}