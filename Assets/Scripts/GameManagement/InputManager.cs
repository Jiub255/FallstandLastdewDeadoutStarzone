using System;
using UnityEngine;

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
    public PlayerControls PC { get; private set; }

    public GameStates GameState { get; private set; }

    // Probably a cleaner way to do this. 
    private GameStates WorldOrBuildMostRecently = GameStates.World;

    private void Awake()
    {
        PC = new PlayerControls();

        // Enable default action maps
        WorldMap();
    }

    // TODO - Implement a game state machine here to control action maps? 
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
        }
    }

    public void OpenInventory(bool open)
    {
        if (open)
        {
            if (GameState == GameStates.World)
            {
                WorldOrBuildMostRecently = GameStates.World;
                ChangeGameState(GameStates.WorldMenus);
            }
            else if (GameState == GameStates.Build)
            {
                WorldOrBuildMostRecently = GameStates.Build;
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
                if (WorldOrBuildMostRecently == GameStates.World)
                {
                    ChangeGameState(GameStates.World);
                }
                else if(WorldOrBuildMostRecently == GameStates.Build)
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

    #region Debug button methods
    public void WorldMap()
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
        PC.BuildCraftingMenus.Enable();
    }
    public void WorldMenusMap()
    {
        PC.Disable();
        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
        PC.BuildCraftingMenus.Enable();
    }
    public void CombatMap()
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.World.Enable();
        PC.InventoryMenu.Enable();
    }
    public void CombatMenusMap()
    {
        PC.Disable();
        PC.Quit.Enable();
        PC.InventoryMenu.Enable();
    }
    public void BuildMap()
    {
        PC.Disable();
        PC.Camera.Enable();
        PC.Quit.Enable();
        PC.Build.Enable();
        PC.InventoryMenu.Enable();
        PC.BuildCraftingMenus.Enable();
    }

    #endregion

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