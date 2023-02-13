using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Maybe put this on TheSingleton?
public class UIManager : MonoBehaviour
{
    // MenuUIRefresher listens to initialize the UI displays
    // Split into separate events for each menu? Or does it really affect performance enough to matter?
    public static event Action OnOpenedMenu;

    // Should this be static? Why? 
    public static bool GamePaused = false;

    [Header("Canvases")]
    [SerializeField]
    private Transform _canvasesObject;
    [SerializeField]
    private GameObject _inventoryCanvas;
    [SerializeField]
    private GameObject _buildCanvas;
    [SerializeField]
    private GameObject _hUDCanvas;

    private void Start()
    {
        S.I.IM.PC.Home.OpenInventory.performed += OpenInventory;
        S.I.IM.PC.Home.OpenBuildMenu.performed += OpenBuildMenu;

        S.I.IM.PC.MenuInventory.CloseInventory.performed += CloseUI;
        S.I.IM.PC.MenuInventory.OpenBuildMenu.performed += OpenBuildMenu;

        S.I.IM.PC.MenuBuild.CloseBuildMenu.performed += CloseUI;
        S.I.IM.PC.MenuBuild.OpenInventory.performed += OpenInventory;

        // Setup all menus in beginning of each scene. (Not sure if necessary)
        // Make sure all UIRefresher child classes subscribe in OnEnable, not Start, so they can hear this. 
        OnOpenedMenu?.Invoke();
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.OpenInventory.performed -= OpenInventory;
        S.I.IM.PC.Home.OpenBuildMenu.performed -= OpenBuildMenu;

        S.I.IM.PC.MenuInventory.CloseInventory.performed -= CloseUI;
        S.I.IM.PC.MenuInventory.OpenBuildMenu.performed -= OpenBuildMenu;

        S.I.IM.PC.MenuBuild.CloseBuildMenu.performed -= CloseUI;
        S.I.IM.PC.MenuBuild.OpenInventory.performed -= OpenInventory;
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
        // Change Action Maps
        S.I.IM.PC.Disable();
        S.I.IM.PC.MenuInventory.Enable();

        // Open inventory canvas
        DeactivateAllCanvases();
        _inventoryCanvas.SetActive(true);

        // InventoryUI listens to setup display
        OnOpenedMenu?.Invoke();

        // Pause gameplay if not already paused
        if (!GamePaused)
        {
            PauseGame();
        }
    }

    private void OpenBuildMenu(InputAction.CallbackContext context)
    {
        // Change Action Maps
        S.I.IM.PC.Disable();
        S.I.IM.PC.World.Enable();
        S.I.IM.PC.MenuBuild.Enable();


        // Open build canvas
        DeactivateAllCanvases();
        _buildCanvas.SetActive(true);

        // BuildUI listens to setup display
        OnOpenedMenu?.Invoke();

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

        DeactivateAllCanvases();
        _hUDCanvas.SetActive(true);

        // PCUI listens to setup display
        OnOpenedMenu?.Invoke();

        if (GamePaused)
        {
            UnpauseGame();
        }
    }

    private void DeactivateAllCanvases()
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