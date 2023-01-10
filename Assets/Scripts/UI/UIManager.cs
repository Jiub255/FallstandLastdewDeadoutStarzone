using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Maybe put this on TheSingleton?
public class UIManager : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField]
    private Transform canvasesObject;
    [SerializeField]
    private Canvas inventoryCanvas;
    [SerializeField]
    private Canvas buildCanvas;

    // MenuUIRefresher listens to initialize the UI displays
    public static event Action onOpenedMenu;

    public static bool gamePaused = false;

    private void Start()
    {
        S.I.InputManager.playerControls.Home.OpenInventory.performed += OpenInventory;
        S.I.InputManager.playerControls.Home.OpenBuildMenu.performed += OpenBuildMenu;
        S.I.InputManager.playerControls.Inventory.CloseInventory.performed += CloseUI;
    }

    private void OnDisable()
    {
        S.I.InputManager.playerControls.Home.OpenInventory.performed -= OpenInventory;
        S.I.InputManager.playerControls.Home.OpenBuildMenu.performed -= OpenBuildMenu;
        S.I.InputManager.playerControls.Inventory.CloseInventory.performed -= CloseUI;
    }

    private void OpenBuildMenu(InputAction.CallbackContext context)
    {
        // Change Action Maps
        S.I.InputManager.playerControls.Disable();
        S.I.InputManager.playerControls.World.Enable();
        S.I.InputManager.playerControls.Build.Enable();

        // Open build canvas
        buildCanvas.gameObject.SetActive(true);

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
        // Change Action Maps
        S.I.InputManager.playerControls.Disable();
        S.I.InputManager.playerControls.Inventory.Enable();

        // Open inventory canvas
        inventoryCanvas.gameObject.SetActive(true);

        // InventoryUI listens to initialize display
        onOpenedMenu.Invoke();

        // Pause gameplay if not already paused
        if (!gamePaused)
        {
            PauseGame();
        }
    }

    private void CloseUI(InputAction.CallbackContext context)
    {
        // Change Action Maps
        S.I.InputManager.playerControls.Disable();
        S.I.InputManager.playerControls.World.Enable();
        S.I.InputManager.playerControls.Home.Enable();

        foreach (Transform canvas in canvasesObject)
        {
            canvas.gameObject.SetActive(false);
        }
       
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
}