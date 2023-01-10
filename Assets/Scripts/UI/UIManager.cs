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
        S.I.IM.PC.Disable();
        S.I.IM.PC.Inventory.Enable();

        // Open inventory canvas
        CloseAllMenus();
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
        S.I.IM.PC.Disable();
        S.I.IM.PC.World.Enable();
        S.I.IM.PC.Home.Enable();

        CloseAllMenus();
       
        if (gamePaused)
        {
            UnpauseGame();
        }
    }

    private void CloseAllMenus()
    {
        foreach (Transform canvas in canvasesObject)
        {
            canvas.gameObject.SetActive(false);
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