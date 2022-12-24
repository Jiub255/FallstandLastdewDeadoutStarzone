using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas inventoryCanvas;

    [SerializeField]
    private InventorySO inventorySO;

/*    private PlayerInput playerInput;
    private InputAction openInventoryAction;
    private InputAction closeInventoryAction;*/

    public static event Action onOpenedInventory;

    public static bool gamePaused = false;

    private void Awake()
    {
/*        playerInput = GetComponent<PlayerInput>();
        openInventoryAction = playerInput.actions["OpenInventory"];
        closeInventoryAction = playerInput.actions["CloseInventory"];*/
    }

    private void Start()
    {
        MasterSingleton.Instance.InputManager.openInventoryAction.performed += OpenInventory;
        MasterSingleton.Instance.InputManager.closeInventoryAction.performed += CloseInventory;
    }

    private void OnDisable()
    {
        MasterSingleton.Instance.InputManager.openInventoryAction.performed -= OpenInventory;
        MasterSingleton.Instance.InputManager.closeInventoryAction.performed -= CloseInventory;
    }

    private void OpenInventory(InputAction.CallbackContext context)
    {
        inventoryCanvas.gameObject.SetActive(true);

        MasterSingleton.Instance.InputManager.ChangeActionMap("UI");
        //playerInput.SwitchCurrentActionMap("UI");

        onOpenedInventory.Invoke();

        gamePaused = true;

        Time.timeScale = 0f;
    }

    private void CloseInventory(InputAction.CallbackContext context)
    {
        inventoryCanvas.gameObject.SetActive(false);

        MasterSingleton.Instance.InputManager.ChangeActionMap("Gameplay");
        //playerInput.SwitchCurrentActionMap("Gameplay");

        gamePaused = false;

        Time.timeScale = 1f;
    }
}