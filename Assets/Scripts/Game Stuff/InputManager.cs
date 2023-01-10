using UnityEngine;

public class InputManager : MonoBehaviour
{
    // From generated C# class
        // Might be easier to use this instead of PlayerInput and every action individually with strings.
        // Just re-generated C# class whenever you change PlayerControl Input Action Asset.
    public PlayerControls playerControls/* = new PlayerControls()*/;

    private void Awake()
    {
        playerControls = new PlayerControls();

        // Enable "Home" and "Gameplay" as default action maps
        playerControls.Disable();
        playerControls.World.Enable();
        playerControls.Home.Enable();
    }

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