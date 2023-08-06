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
    // Still doing singleton input manager for now though. 
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

        inputManager.PC.InventoryMenu.OpenInventory.started += OpenInventory;
        inputManager.PC.InventoryMenu.CloseInventory.started += CloseUI;
        inputManager.PC.NonCombatMenus.OpenBuildMenu.started += OpenBuildMenu;
        inputManager.PC.NonCombatMenus.CloseBuildMenu.started += CloseUI;
        inputManager.PC.NonCombatMenus.OpenCraftingMenu.started += OpenCraftingMenu;
        inputManager.PC.NonCombatMenus.CloseCraftingMenu.started += CloseUI;
        inputManager.PC.NonCombatMenus.OpenMap.started += OpenMap;
        inputManager.PC.NonCombatMenus.CloseMap.started += CloseUI;
    }

    private void OnDisable()
    {
        GameManager.OnInputManagerCreated -= SetupInput;
        GameManager.OnGameStateMachineCreated -= (gameStateMachine) => _gameStateMachine = gameStateMachine;

        InputManager.PC.InventoryMenu.OpenInventory.started -= OpenInventory;
        InputManager.PC.InventoryMenu.CloseInventory.started -= CloseUI;
        InputManager.PC.NonCombatMenus.OpenBuildMenu.started -= OpenBuildMenu;
        InputManager.PC.NonCombatMenus.CloseBuildMenu.started -= CloseUI;
        InputManager.PC.NonCombatMenus.OpenCraftingMenu.started -= OpenCraftingMenu;
        InputManager.PC.NonCombatMenus.CloseCraftingMenu.started -= CloseUI;
        InputManager.PC.NonCombatMenus.OpenMap.started -= OpenMap;
        InputManager.PC.NonCombatMenus.CloseMap.started -= CloseUI;
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
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

    private void OpenBuildMenu(InputAction.CallbackContext context)
    {
        _gameStateMachine.ChangeGameStateTo(_gameStateMachine.Build());
        
        OpenMenu(_buildCanvas);
    }

    private void OpenCraftingMenu(InputAction.CallbackContext context)
    {
        _gameStateMachine.ChangeGameStateTo(_gameStateMachine.HomeMenus());

        OpenMenu(_craftingCanvas);
    }

    private void OpenMap(InputAction.CallbackContext context)
    {
        _gameStateMachine.ChangeGameStateTo(_gameStateMachine.HomeMenus());

        OpenMenu(_mapCanvas);
    }

    private void OpenMenu(GameObject canvas)
    {
        OpenCanvas(canvas);
    }

    private void CloseUI(InputAction.CallbackContext context)
    {
        if (_gameStateMachine.ActiveState.GetType() == typeof(GameHomeMenusState))
        {
            _gameStateMachine.ChangeGameStateTo(_gameStateMachine.Home());
        }
        else if (_gameStateMachine.ActiveState.GetType() == typeof(GameCombatMenusState))
        {
            _gameStateMachine.ChangeGameStateTo(_gameStateMachine.Combat());
        }

        OpenCanvas(_hUDCanvas);
    }

    private void OpenCanvas(GameObject canvas)
    {
        if (_buildCanvas.activeInHierarchy) _buildCanvas.gameObject.SetActive(false);
        if (_craftingCanvas.activeInHierarchy) _craftingCanvas.gameObject.SetActive(false);
        if (_homeInventoryCanvas.activeInHierarchy) _homeInventoryCanvas.gameObject.SetActive(false);
        if (_hUDCanvas.activeInHierarchy) _hUDCanvas.gameObject.SetActive(false);
        if (!canvas.activeInHierarchy) canvas.SetActive(true);
    }
}