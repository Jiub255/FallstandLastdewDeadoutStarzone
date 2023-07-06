using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Put this on Canvases object. 
public class UIManager : MonoBehaviour
{
    // TODO - Do this better. Send individual events for menus, don't rebuild entire UI each time. 
    // MenuUIRefresher listens to initialize the UI displays
    // Split into separate events for each menu? Or does it really affect performance enough to matter?
    public static event Action OnOpenedMenu;

    [Header("Canvases")]
    [SerializeField]
    private GameObject _inventoryCanvas;
    [SerializeField]
    private GameObject _buildCanvas;
    [SerializeField]
    private GameObject _craftingCanvas;
    [SerializeField]
    private GameObject _hUDCanvas;

    private GameManager _gameManager;
    private InputManager _inputManager;

    private void Start()
    {
        _gameManager = S.I.GameManager;
        _inputManager = S.I.IM;

        // TODO - Put menu control actions in one map, since OpenCanvas checks if things are already active/inactive. 
        _inputManager.PC.InventoryMenu.OpenInventory.performed += OpenInventory;
        _inputManager.PC.InventoryMenu.CloseInventory.performed += CloseUI;
        _inputManager.PC.BuildCraftingMenus.OpenBuildMenu.performed += OpenBuildMenu;
        _inputManager.PC.BuildCraftingMenus.CloseBuildMenu.performed += CloseUI;

        // Setup all menus in beginning of each scene. (Not sure if necessary)
        // Make sure all UIRefresher child classes subscribe in OnEnable, not Start, so they can hear this. 
        OnOpenedMenu?.Invoke();
    }

    private void OnDisable()
    {
        _inputManager.PC.InventoryMenu.OpenInventory.performed -= OpenInventory;
        _inputManager.PC.InventoryMenu.CloseInventory.performed -= CloseUI;
        _inputManager.PC.BuildCraftingMenus.OpenBuildMenu.performed -= OpenBuildMenu;
        _inputManager.PC.BuildCraftingMenus.CloseBuildMenu.performed -= CloseUI;
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
        // Change Action Maps
        _inputManager.OpenInventory(true);

        OpenCanvas(_inventoryCanvas);

        // InventoryUI listens to setup display
        OnOpenedMenu?.Invoke();

        // Pause gameplay. 
        _gameManager.Pause(true);
    }

    private void OpenBuildMenu(InputAction.CallbackContext context)
    {
        // Change Action Maps
        _inputManager.OpenInventory(true);

        OpenCanvas(_buildCanvas);

        // BuildUI listens to setup display
        OnOpenedMenu?.Invoke();

        // Pause gameplay. 
        _gameManager.Pause(true);
    }

    private void OpenCraftingMenu(InputAction.CallbackContext context)
    {
        // Change Action Maps
        _inputManager.OpenInventory(true);

        OpenCanvas(_craftingCanvas);

        // CanvasUI listens to setup display
        OnOpenedMenu?.Invoke();

        // Pause gameplay. 
        _gameManager.Pause(true);
    }

    private void CloseUI(InputAction.CallbackContext context)
    {
        // Change Action Maps
        _inputManager.OpenInventory(false);

        OpenCanvas(_hUDCanvas);

        // PCUI listens to setup display
        OnOpenedMenu?.Invoke();

        // Unpause gameplay. 
        _gameManager.Pause(false);
    }

    private void OpenCanvas(GameObject canvas)
    {
        if (_buildCanvas.activeInHierarchy) _buildCanvas.gameObject.SetActive(false);
        if (_craftingCanvas.activeInHierarchy) _craftingCanvas.gameObject.SetActive(false);
        if (_inventoryCanvas.activeInHierarchy) _inventoryCanvas.gameObject.SetActive(false);
        if (_hUDCanvas.activeInHierarchy) _hUDCanvas.gameObject.SetActive(false);
        if (!canvas.activeInHierarchy) canvas.SetActive(true);
    }
}