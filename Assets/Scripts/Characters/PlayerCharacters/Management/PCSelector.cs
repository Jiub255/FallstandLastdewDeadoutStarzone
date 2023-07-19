using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PCSelector : MonoBehaviour
{
    public static event Action<Transform> OnDoubleClickPC;

    [SerializeField]
    private SOListSOPC _currentTeamSO; 

    [SerializeField]
    private LayerMask _pcLayerMask;

    [SerializeField]
    private float _doubleClickTimeLimit = 0.5f;

    private EventSystem _eventSystem;
    private bool _pointerOverUI = false;
    private float _lastClickTime = 0f;
    private int _firstClickedObjectID;
    private InputAction _mousePositionAction;

    private void Start()
    {
        _eventSystem = EventSystem.current;
        _mousePositionAction = S.I.IM.PC.Camera.MousePosition;

        // Click on PC icon. 
        SOPC.OnSelectPC += HandleClick;

        // Click on PC instance. 
        S.I.IM.PC.World.SelectOrCenter.canceled/*performed*/ += CheckIfPCClicked;
        
        PlayerIdleState.OnPCDeselected += () => ChangePC(null);
    }

    private void OnDisable()
    {
        // Click on PC icon. 
        SOPC.OnSelectPC -= HandleClick;

        // Click on PC instance. 
        S.I.IM.PC.World.SelectOrCenter.canceled/*performed*/ -= CheckIfPCClicked;

        PlayerIdleState.OnPCDeselected -= () => ChangePC(null);
    }

    private void Update()
    {
        _pointerOverUI = _eventSystem.IsPointerOverGameObject();
    }

    private void CheckIfPCClicked(InputAction.CallbackContext context)
    {
        if (!_pointerOverUI)
        {
            // Only raycast to PC layer. 
            RaycastHit[] hits = Physics.RaycastAll(
                Camera.main.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>()),
                1000,
                _pcLayerMask);

            // If there were any hits, they must have been PCs. 
            if (hits.Length > 0)
            {
                // This handles double/single clicking. 
                HandleClick(hits[0].transform.gameObject);
            }
        }
    }

    // Still get double click if you click on two different buttons within time limit?
    private void HandleClick(GameObject pcInstance)
    {
        float currentClickTime = Time.realtimeSinceStartup;

        // Double click 
        if ((currentClickTime - _lastClickTime) < _doubleClickTimeLimit)
        {
            Debug.Log("Double click");
            // If the second click was on the same PC as first click, center on that PC. 
            if (pcInstance.GetInstanceID() == _firstClickedObjectID)
            {
                // CameraMoveRotate listens, centers on PC. 
                OnDoubleClickPC?.Invoke(pcInstance.transform);
            }
            // If second click was on a different PC, treat it like a single click. 
            else
            {
                // Select PC. 
                ChangePC(pcInstance); 

                // Get reference to first object clicked, to compare on second click. 
                _firstClickedObjectID = pcInstance.GetInstanceID(); 
            }
        }
        // Single Click 
        else
        {
            Debug.Log("Single click");
            // Select PC. 
            ChangePC(pcInstance); 

            // Get reference to first object clicked, to compare on second click. 
            _firstClickedObjectID = pcInstance.GetInstanceID(); 
        }

        _lastClickTime = currentClickTime;
    }

    private void ChangePC(GameObject clickedPCInstance)
    {
        // If clicked PC is NOT your selected PC, or selected PC is null, 
        if (_currentTeamSO.SelectedPC == null || _currentTeamSO.SelectedPC != clickedPCInstance)
        {
            // If there is a currently selected PC, set its Selected to false. 
            if (_currentTeamSO.SelectedPC != null)
            {
                _currentTeamSO.SelectedPC.GetComponent<PlayerController>().SetSelected(false);
            }

            // Set clicked PC's Selected to true.
            if (clickedPCInstance != null)
            {
                clickedPCInstance.GetComponent<PlayerController>().SetSelected(true);
            }
         
            _currentTeamSO.SelectedPC = clickedPCInstance;
            // Also set PC as current menu PC so you always see your most recently selected character when you open the inventory. 
            if (clickedPCInstance != null)
            {
                _currentTeamSO.CurrentMenuSOPC = clickedPCInstance.GetComponentInChildren<PCStatManager>().PCSO;
            }
        }
    }
}