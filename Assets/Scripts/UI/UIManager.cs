using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Maybe put this on TheSingleton?
public class UIManager : MonoBehaviour
{
    // MenuUIRefresher listens to initialize the UI displays
    public static event Action OnOpenedMenu;

    public static bool GamePaused = false;

    [Header("Canvases")]
    [SerializeField]
    private Transform _canvasesObject;
    [SerializeField]
    private Canvas _inventoryCanvas;
    [SerializeField]
    private Canvas _buildCanvas;

    private void Start()
    {
        S.I.IM.PC.Home.OpenInventory.performed += OpenInventory;
        S.I.IM.PC.Home.OpenBuildMenu.performed += OpenBuildMenu;
        S.I.IM.PC.Inventory.CloseInventory.performed += CloseUI;
        S.I.IM.PC.Inventory.OpenBuildMenu.performed += OpenBuildMenu;
        S.I.IM.PC.Build.CloseBuildMenu.performed += CloseUI;
        S.I.IM.PC.Build.OpenInventory.performed += OpenInventory;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.OpenInventory.performed -= OpenInventory;
        S.I.IM.PC.Home.OpenBuildMenu.performed -= OpenBuildMenu;
        S.I.IM.PC.Inventory.CloseInventory.performed -= CloseUI;
        S.I.IM.PC.Inventory.OpenBuildMenu.performed -= OpenBuildMenu;
        S.I.IM.PC.Build.CloseBuildMenu.performed -= CloseUI;
        S.I.IM.PC.Build.OpenInventory.performed -= OpenInventory;
    }

    private void OpenBuildMenu(InputAction.CallbackContext context)
    {
        // Change Action Maps
        S.I.IM.PC.Disable();
        S.I.IM.PC.World.Enable();
        S.I.IM.PC.Build.Enable();

        // Open build canvas
        CloseAllMenus();
        _buildCanvas.gameObject.SetActive(true);

        // BuildMenuUI listens to initialize display
        OnOpenedMenu.Invoke();

        // Pause gameplay if not already paused
        if (!GamePaused)
        {
            PauseGame();
        }
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
        // Change Action Maps
        S.I.IM.PC.Disable();
        S.I.IM.PC.Inventory.Enable();

        // Open inventory canvas
        CloseAllMenus();
        _inventoryCanvas.gameObject.SetActive(true);

        // InventoryUI listens to initialize display
        OnOpenedMenu.Invoke();

        // Pause gameplay if not already paused
        if (!GamePaused)
        {
            PauseGame();
        }
    }

    private void CloseUI(InputAction.CallbackContext context)
    {
        // Change Action Maps
        S.I.IM.PC.Disable();
        S.I.IM.PC.World.Enable();
        S.I.IM.PC.Home.Enable();

        CloseAllMenus();
       
        if (GamePaused)
        {
            UnpauseGame();
        }
    }

    private void CloseAllMenus()
    {
        foreach (Transform canvas in _canvasesObject)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    public static void PauseGame()
    {
        GamePaused = true;

        Time.timeScale = 0f;
    }

    public static void UnpauseGame()
    {
        GamePaused = false;

        Time.timeScale = 1f;
    }
}