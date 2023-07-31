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
    private GameStateMachine _gameManager;

    private void Start()
    {
        _gameManager = S.I.GSM;

        S.I.IM.PC.InventoryMenu.OpenInventory.started += OpenInventory;
        S.I.IM.PC.InventoryMenu.CloseInventory.started += CloseUI;
        S.I.IM.PC.NonCombatMenus.OpenBuildMenu.started += OpenBuildMenu;
        S.I.IM.PC.NonCombatMenus.CloseBuildMenu.started += CloseUI;
        S.I.IM.PC.NonCombatMenus.OpenCraftingMenu.started += OpenCraftingMenu;
        S.I.IM.PC.NonCombatMenus.CloseCraftingMenu.started += CloseUI;
        S.I.IM.PC.NonCombatMenus.OpenMap.started += OpenMap;
        S.I.IM.PC.NonCombatMenus.CloseMap.started += CloseUI;
    }

    private void OnDisable()
    {
        S.I.IM.PC.InventoryMenu.OpenInventory.started -= OpenInventory;
        S.I.IM.PC.InventoryMenu.CloseInventory.started -= CloseUI;
        S.I.IM.PC.NonCombatMenus.OpenBuildMenu.started -= OpenBuildMenu;
        S.I.IM.PC.NonCombatMenus.CloseBuildMenu.started -= CloseUI;
        S.I.IM.PC.NonCombatMenus.OpenCraftingMenu.started -= OpenCraftingMenu;
        S.I.IM.PC.NonCombatMenus.CloseCraftingMenu.started -= CloseUI;
        S.I.IM.PC.NonCombatMenus.OpenMap.started -= OpenMap;
        S.I.IM.PC.NonCombatMenus.CloseMap.started -= CloseUI;
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
        if (_gameManager.ActiveState.GetType() == typeof(GameHomeState) ||
            _gameManager.ActiveState.GetType() == typeof(GameBuildState))
        {
            _gameManager.ChangeGameStateTo(_gameManager.HomeMenus());

            OpenMenu(_homeInventoryCanvas);
        }
        else if (_gameManager.ActiveState.GetType() == typeof(GameCombatState))
        {
            _gameManager.ChangeGameStateTo(_gameManager.CombatMenus());

            OpenMenu(_combatInventoryCanvas);
        }
    }

    private void OpenBuildMenu(InputAction.CallbackContext context)
    {
        _gameManager.ChangeGameStateTo(_gameManager.Build());
        
        OpenMenu(_buildCanvas);
    }

    private void OpenCraftingMenu(InputAction.CallbackContext context)
    {
        _gameManager.ChangeGameStateTo(_gameManager.HomeMenus());

        OpenMenu(_craftingCanvas);
    }

    private void OpenMap(InputAction.CallbackContext context)
    {
        _gameManager.ChangeGameStateTo(_gameManager.HomeMenus());

        OpenMenu(_mapCanvas);
    }

    private void OpenMenu(GameObject canvas)
    {
        OpenCanvas(canvas);
    }

    private void CloseUI(InputAction.CallbackContext context)
    {
        if (_gameManager.ActiveState.GetType() == typeof(GameHomeMenusState))
        {
            _gameManager.ChangeGameStateTo(_gameManager.Home());
        }
        else if (S.I.GSM.ActiveState.GetType() == typeof(GameCombatMenusState))
        {
            _gameManager.ChangeGameStateTo(_gameManager.Combat());
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