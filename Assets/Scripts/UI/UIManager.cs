using UnityEngine;
using UnityEngine.InputSystem;

// Put this on Canvases object. 
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

    private GameStateMachine _gameManager;
    private InputManager _inputManager;

    private void Start()
    {
        _gameManager = S.I.GSM;
        _inputManager = S.I.IM;

        _inputManager.PC.InventoryMenu.OpenInventory.started += OpenInventory;
        _inputManager.PC.InventoryMenu.CloseInventory.started += CloseUI;
        _inputManager.PC.NonCombatMenus.OpenBuildMenu.started += OpenBuildMenu;
        _inputManager.PC.NonCombatMenus.CloseBuildMenu.started += CloseUI;
        _inputManager.PC.NonCombatMenus.OpenCraftingMenu.started += OpenCraftingMenu;
        _inputManager.PC.NonCombatMenus.CloseCraftingMenu.started += CloseUI;
        _inputManager.PC.NonCombatMenus.OpenMap.started += OpenMap;
        _inputManager.PC.NonCombatMenus.CloseMap.started += CloseUI;
    }

    private void OnDisable()
    {
        _inputManager.PC.InventoryMenu.OpenInventory.started -= OpenInventory;
        _inputManager.PC.InventoryMenu.CloseInventory.started -= CloseUI;
        _inputManager.PC.NonCombatMenus.OpenBuildMenu.started -= OpenBuildMenu;
        _inputManager.PC.NonCombatMenus.CloseBuildMenu.started -= CloseUI;
        _inputManager.PC.NonCombatMenus.OpenCraftingMenu.started -= OpenCraftingMenu;
        _inputManager.PC.NonCombatMenus.CloseCraftingMenu.started -= CloseUI;
        _inputManager.PC.NonCombatMenus.OpenMap.started -= OpenMap;
        _inputManager.PC.NonCombatMenus.CloseMap.started -= CloseUI;
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
        if (S.I.GSM.ActiveState.GetType() == typeof(GameHomeState) || 
            S.I.GSM.ActiveState.GetType() == typeof(GameBuildState))
        {
            S.I.GSM.ChangeGameStateTo(S.I.GSM.HomeMenus());

            OpenMenu(_homeInventoryCanvas);
        }
        else if (S.I.GSM.ActiveState.GetType() == typeof(GameCombatState))
        {
            S.I.GSM.ChangeGameStateTo(S.I.GSM.CombatMenus());

            OpenMenu(_combatInventoryCanvas);
        }
    }

    private void OpenBuildMenu(InputAction.CallbackContext context)
    {
        S.I.GSM.ChangeGameStateTo(S.I.GSM.Build());
        
        OpenMenu(_buildCanvas);
    }

    private void OpenCraftingMenu(InputAction.CallbackContext context)
    {
        S.I.GSM.ChangeGameStateTo(S.I.GSM.HomeMenus());

        OpenMenu(_craftingCanvas);
    }

    private void OpenMap(InputAction.CallbackContext context)
    {
        S.I.GSM.ChangeGameStateTo(S.I.GSM.HomeMenus());

        OpenMenu(_mapCanvas);
    }

    private void OpenMenu(GameObject canvas)
    {
        OpenCanvas(canvas);
    }

    private void CloseUI(InputAction.CallbackContext context)
    {
        if (S.I.GSM.ActiveState.GetType() == typeof(GameHomeMenusState))
        {
            S.I.GSM.ChangeGameStateTo(S.I.GSM.Home());
        }
        else if (S.I.GSM.ActiveState.GetType() == typeof(GameCombatMenusState))
        {
            S.I.GSM.ChangeGameStateTo(S.I.GSM.Combat());
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