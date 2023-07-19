using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameStates
{
    Home,
    HomeMenus,
    Combat,
    CombatMenus,
    Build
}

public class InputManager : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext> OnDeselectOrCancel;

    public PlayerControls PC { get; private set; }

    public GameStates GameState { get; private set; }

    // Probably a cleaner way to do this. 
    // Maybe do a proper game state machine? 
    // Have it control which menus/scenes are open, and which controls to go with them. Would be 
    // cleaner and less breakable than this. 
    // Also will make it a lot easier to temporarily disable all controls (while loading between scenes, etc). 
    private GameStates MostRecentActionMap = GameStates.Home;

    private Vector2 _startingMousePosition;
    [SerializeField]
    private float _mouseMovementThreshold = 0.1f;
    private float _mouseMovementThresholdSquared { get { return _mouseMovementThreshold * _mouseMovementThreshold; } }

    private void Awake()
    {
        PC = new PlayerControls();

        // Enable default action maps
        ChangeGameState(GameStates.Home);

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

    public void DisableAllActions()
    {
        PC.Disable();
    }

    public void ChangeGameState(GameStates gameState)
    {
        GameState = gameState;
        ActivateStateActionMaps(gameState);
    }

    public void EnableStateActionMaps(GameState gameState)
    {
        EnableStatesActionMaps(gameState as dynamic);
    }

    private void EnableStatesActionMaps(GamePauseState pauseState)
    {
        PC.Disable();
    }








    /// <summary>
    /// Activates all action maps for gameState. 
    /// </summary>
    /// <param name="gameState"></param>
    public void ActivateStateActionMaps(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.Home:
                HomeActionMaps();
                break;
            case GameStates.HomeMenus:
                HomeMenusActionMaps();
                break;
            case GameStates.Combat:
                CombatActionMaps();
                break;
            case GameStates.CombatMenus:
                CombatMenusActionMaps();
                break;
            case GameStates.Build:
                BuildActionMaps();
                break;
            default:
                Debug.Log($"No state matching {gameState} found. Add {gameState} to InputManager.ActivateStateActionMaps(). ");
                break;
        }
    }

    public void OpenInventory(bool open)
    {
        if (open)
        {
            if (GameState == GameStates.Home)
            {
                MostRecentActionMap = GameStates.Home;
                ChangeGameState(GameStates.HomeMenus);
            }
            else if (GameState == GameStates.Build)
            {
                MostRecentActionMap = GameStates.Build;
                ChangeGameState(GameStates.HomeMenus);
            }
            else if(GameState == GameStates.Combat)
            {
                ChangeGameState(GameStates.CombatMenus);
            }
        }
        else
        {
            if (GameState == GameStates.HomeMenus)
            {
                if (MostRecentActionMap == GameStates.Home)
                {
                    ChangeGameState(GameStates.Home);
                }
                else if(MostRecentActionMap == GameStates.Build)
                {
                    ChangeGameState(GameStates.Build);
                }
            }
            else if (GameState == GameStates.CombatMenus)
            {
                ChangeGameState(GameStates.Combat);
            }
        }
    }

    // Used at home base (not during combat/raids/invasions).
    public void HomeActionMaps()
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }

    // Used in non-combat menus. 
    public void HomeMenusActionMaps()
    {
        PC.Disable();
        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }

    // Used in scavenging scenes and home base combat. 
    public void CombatActionMaps()
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
    }

    // Used in combat menus. 
    public void CombatMenusActionMaps()
    {
        PC.Disable();
        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
    }

    // Used in build mode in home base. 
    public void BuildActionMaps()
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.Build.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }

    // TODO: Need to deactivate player movement/currentlySelectedPC when going into build mode.

    // Menu -> Action Maps (UI automatically used with canvas stuff, through event system
    //-----------------------------
    // HOME
    // No Menu -> World, Home
    // Inventory -> Inventory
    // Build -> World, Build

    // SCAVENGE
    // No Menu -> World, Scavenge
    // Inventory -> Inventory
    // Character Status -> Status
}