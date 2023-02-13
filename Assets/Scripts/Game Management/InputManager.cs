using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerControls PC { get; private set; }

    private void Awake()
    {
        PC = new PlayerControls();

        // Enable "Home" and "Gameplay" as default action maps
        PC.Disable();
        PC.World.Enable();
        PC.Home.Enable();
    }

    #region Debug button methods
    public void HomeMap()
    {
        PC.Disable();
        PC.World.Enable();
        PC.Home.Enable();
    }
    public void ScavengeMap()
    {
        PC.Disable();
        PC.World.Enable();
        PC.Scavenge.Enable();
    }
    public void HomeScavengeMap()
    {
        PC.Disable();
        PC.World.Enable();
        PC.Home.Enable();
        PC.Scavenge.Enable();
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