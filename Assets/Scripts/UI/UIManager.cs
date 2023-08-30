using UnityEngine;
using UnityEngine.InputSystem;

// Put this on Canvases object. 
/// <summary>
/// TODO - Put this on the GameManager as a non-MB, and get the canvas references as they get instantiated from here? <br/>
/// Also have this class keep references to all the other UI scripts as non-MBs too? 
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField, Header("Canvases")]
    private GameObject _homeInventoryCanvas;
    [SerializeField]
    private GameObject _combatInventoryCanvas;
    [SerializeField]
    private GameObject _buildCanvas;
    [SerializeField]
    private GameObject _craftingCanvas;
    [SerializeField]
    private GameObject _mapCanvas;
    [SerializeField]
    private GameObject _hUDCanvas;

    // TODO - Handle this differently. Put GameState in SOGameData, and use events to call upwards. 
    private GameStateMachine _gameStateMachine;
    private InputManager InputManager { get; set; }

    private void OnEnable()
    {
        GameManager.OnInputManagerCreated += SetupInput;
        GameManager.OnGameStateMachineCreated += (gameStateMachine) => _gameStateMachine = gameStateMachine;
    }

    private void SetupInput(InputManager inputManager)
    {
        InputManager = inputManager;

        inputManager.PC.InventoryMenu.ToggleInventory.started += ToggleInventory;
        inputManager.PC.InventoryMenu.CloseMenus.started += CloseUI;
        inputManager.PC.NonCombatMenus.ToggleBuildMenu.started += ToggleBuildMenu;
        inputManager.PC.NonCombatMenus.ToggleCraftingMenu.started += ToggleCraftingMenu;
        inputManager.PC.NonCombatMenus.ToggleMapMenu.started += ToggleMapMenu;
        inputManager.PC.NonCombatMenus.CloseMenus.started += CloseUI;

/*        inputManager.PC.NonCombatMenus.ToggleBuildMenu.started += (c) => Debug.Log("started");
        inputManager.PC.NonCombatMenus.ToggleBuildMenu.performed += (c) => Debug.Log("performed");
        inputManager.PC.NonCombatMenus.ToggleBuildMenu.canceled += (c) => Debug.Log("canceled");*/

/*        inputManager.PC.InventoryMenu.OpenInventory.started += OpenInventory;
        inputManager.PC.InventoryMenu.CloseInventory.started += CloseUI;
        inputManager.PC.NonCombatMenus.OpenBuildMenu.started += OpenBuildMenu;
        inputManager.PC.NonCombatMenus.CloseBuildMenu.started += CloseUI;
        inputManager.PC.NonCombatMenus.OpenCraftingMenu.started += OpenCraftingMenu;
        inputManager.PC.NonCombatMenus.CloseCraftingMenu.started += CloseUI;
        inputManager.PC.NonCombatMenus.OpenMap.started += OpenMap;
        inputManager.PC.NonCombatMenus.CloseMap.started += CloseUI;*/
    }

    private void OnDisable()
    {
        GameManager.OnInputManagerCreated -= SetupInput;
        GameManager.OnGameStateMachineCreated -= (gameStateMachine) => _gameStateMachine = gameStateMachine;

        InputManager.PC.InventoryMenu.ToggleInventory.started -= ToggleInventory;
        InputManager.PC.InventoryMenu.CloseMenus.started -= CloseUI;
        InputManager.PC.NonCombatMenus.ToggleBuildMenu.started -= ToggleBuildMenu;
        InputManager.PC.NonCombatMenus.ToggleCraftingMenu.started -= ToggleCraftingMenu;
        InputManager.PC.NonCombatMenus.ToggleMapMenu.started -= ToggleMapMenu;
        InputManager.PC.NonCombatMenus.CloseMenus.started -= CloseUI;

/*        InputManager.PC.NonCombatMenus.ToggleBuildMenu.started -= (c) => Debug.Log("started");
        InputManager.PC.NonCombatMenus.ToggleBuildMenu.performed -= (c) => Debug.Log("performed");
        InputManager.PC.NonCombatMenus.ToggleBuildMenu.canceled -= (c) => Debug.Log("canceled");*/

        /*        InputManager.PC.InventoryMenu.OpenInventory.started -= OpenInventory;
                InputManager.PC.InventoryMenu.CloseInventory.started -= CloseUI;
                InputManager.PC.NonCombatMenus.OpenBuildMenu.started -= OpenBuildMenu;
                InputManager.PC.NonCombatMenus.CloseBuildMenu.started -= CloseUI;
                InputManager.PC.NonCombatMenus.OpenCraftingMenu.started -= OpenCraftingMenu;
                InputManager.PC.NonCombatMenus.CloseCraftingMenu.started -= CloseUI;
                InputManager.PC.NonCombatMenus.OpenMap.started -= OpenMap;
                InputManager.PC.NonCombatMenus.CloseMap.started -= CloseUI;*/
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        // If already in inventory, close it. 
        if (_gameStateMachine.ActiveState.GetType() == typeof(GameHomeMenusState) ||
            _gameStateMachine.ActiveState.GetType() == typeof(GameCombatMenusState))
        {
            Debug.Log("ToggleInventory close");
            CloseUI(context);
        }
        // Otherwise open inventory.
        else
        {
            Debug.Log("ToggleInventory open");
            if (_gameStateMachine.ActiveState.GetType() == typeof(GameHomeState) ||
                _gameStateMachine.ActiveState.GetType() == typeof(GameBuildState))
            {
                _gameStateMachine.ChangeGameStateTo(_gameStateMachine.HomeMenus());

                OpenMenu(_homeInventoryCanvas);
            }
            else if (_gameStateMachine.ActiveState.GetType() == typeof(GameCombatState))
            {
                _gameStateMachine.ChangeGameStateTo(_gameStateMachine.CombatMenus());

                OpenMenu(_combatInventoryCanvas);
            }
        }
    }

    // TODO - Change in game state causing button to trigger again? Or something like that? YES, FIX IT. 
    private void ToggleBuildMenu(InputAction.CallbackContext context)
    {
        // Only open if not in combat. 
        if (_gameStateMachine.ActiveState.GetType() != typeof(GameCombatMenusState) &&
            _gameStateMachine.ActiveState.GetType() != typeof(GameCombatState))
        {
            // Open build menu if not already open.
            if (!_buildCanvas.activeInHierarchy)
            {
                Debug.Log("ToggleBuildMenu open");
                _gameStateMachine.ChangeGameStateTo(_gameStateMachine.Build());
        
                OpenMenu(_buildCanvas);
            }
            else
            {
                Debug.Log("ToggleBuildMenu close");
                CloseUI(context);
            }
        }
    }

    private void ToggleCraftingMenu(InputAction.CallbackContext context)
    {
        // Only open if not in combat. 
        if (_gameStateMachine.ActiveState.GetType() != typeof(GameCombatMenusState) &&
            _gameStateMachine.ActiveState.GetType() != typeof(GameCombatState))
        {
            // Open crafting menu if not already open.
            if (!_craftingCanvas.activeInHierarchy)
            {
                Debug.Log("ToggleCraftingMenu open");
                _gameStateMachine.ChangeGameStateTo(_gameStateMachine.HomeMenus());

                OpenMenu(_craftingCanvas);
            }
            else
            {
                Debug.Log("ToggleCraftingMenu close");
                CloseUI(context);
            }
        }
    }

    private void ToggleMapMenu(InputAction.CallbackContext context)
    {
        // Only open if not in combat. 
        if (_gameStateMachine.ActiveState.GetType() != typeof(GameCombatMenusState) &&
            _gameStateMachine.ActiveState.GetType() != typeof(GameCombatState))
        {
            // Open map menu if not already open. 
            if (!_mapCanvas.activeInHierarchy)
            {
                Debug.Log("ToggleMapMenu open");
                _gameStateMachine.ChangeGameStateTo(_gameStateMachine.HomeMenus());

                OpenMenu(_mapCanvas);
            }
            else
            {
                Debug.Log("ToggleMapMenu close");
                CloseUI(context);
            }
        }
    }

    private void CloseUI(InputAction.CallbackContext context)
    {
        if (_gameStateMachine.ActiveState.GetType() == typeof(GameHomeMenusState) ||
            _gameStateMachine.ActiveState.GetType() == typeof(GameBuildState))
        {
            _gameStateMachine.ChangeGameStateTo(_gameStateMachine.Home());
        }
        else if (_gameStateMachine.ActiveState.GetType() == typeof(GameCombatMenusState))
        {
            _gameStateMachine.ChangeGameStateTo(_gameStateMachine.Combat());
        }

        OpenMenu(_hUDCanvas);
    }

    /// <summary>
    /// Closes all canvases except for "canvas".
    /// </summary>
    private void OpenMenu(GameObject canvas)
    {
        if (_homeInventoryCanvas.activeInHierarchy) _homeInventoryCanvas.SetActive(false);
        if (_combatInventoryCanvas.activeInHierarchy) _combatInventoryCanvas.SetActive(false);
        if (_buildCanvas.activeInHierarchy) _buildCanvas.SetActive(false);
        if (_craftingCanvas.activeInHierarchy) _craftingCanvas.SetActive(false);
        if (_mapCanvas.activeInHierarchy) _mapCanvas.SetActive(false);
        if (_hUDCanvas.activeInHierarchy) _hUDCanvas.SetActive(false);
        if (!canvas.activeInHierarchy) canvas.SetActive(true);
    }
}