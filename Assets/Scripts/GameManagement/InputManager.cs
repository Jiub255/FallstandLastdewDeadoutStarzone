using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameStates
{
    World,
    WorldMenus,
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
    private GameStates MostRecentActionMap = GameStates.World;

    private Vector2 _startingMousePosition;
    [SerializeField]
    private float _mouseMovementThreshold = 0.1f;
    private float _mouseMovementThresholdSquared { get { return _mouseMovementThreshold * _mouseMovementThreshold; } }

    private void Awake()
    {
        PC = new PlayerControls();

        // Enable default action maps
        ChangeGameState(GameStates.World);

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

    public void HandleRightClick(InputAction.CallbackContext context)
    {
        if ((PC.Camera.MousePosition.ReadValue<Vector2>() - _startingMousePosition).sqrMagnitude < _mouseMovementThresholdSquared)
        {
            OnDeselectOrCancel?.Invoke(context);
        }
    }

    public void ChangeGameState(GameStates gameState)
    {
        GameState = gameState;
        ActivateStateActionMaps(gameState);
    }

    private void ActivateStateActionMaps(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.World:
                WorldMap();
                break;
            case GameStates.WorldMenus:
                WorldMenusMap();
                break;
            case GameStates.Combat:
                CombatMap();
                break;
            case GameStates.CombatMenus:
                CombatMenusMap();
                break;
            case GameStates.Build:
                BuildMap();
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
            if (GameState == GameStates.World)
            {
                MostRecentActionMap = GameStates.World;
                ChangeGameState(GameStates.WorldMenus);
            }
            else if (GameState == GameStates.Build)
            {
                MostRecentActionMap = GameStates.Build;
                ChangeGameState(GameStates.WorldMenus);
            }
            else if(GameState == GameStates.Combat)
            {
                ChangeGameState(GameStates.CombatMenus);
            }
        }
        else
        {
            if (GameState == GameStates.WorldMenus)
            {
                if (MostRecentActionMap == GameStates.World)
                {
                    ChangeGameState(GameStates.World);
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
    public void WorldMap()
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }

    // Used in non-combat menus. 
    public void WorldMenusMap()
    {
        PC.Disable();
        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
        PC.NonCombatMenus.Enable();
    }

    // Used in scavenging scenes and home base combat. 
    public void CombatMap()
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
    }

    // Used in combat menus. 
    public void CombatMenusMap()
    {
        PC.Disable();
        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
    }

    // Used in build mode in home base. 
    public void BuildMap()
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