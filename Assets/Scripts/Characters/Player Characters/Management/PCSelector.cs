using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PCSelector : MonoBehaviour
{
    public static event Action<Transform> OnDoubleClickPC;

/*    [SerializeField]
    private GOListSO _pcInstancesSO;*/

    [SerializeField]
    private LayerMask _pcLayerMask;

    [SerializeField]
    private float _doubleClickTimeLimit = 0.5f;
    private float _lastClickTime = 0f;

    private EventSystem _eventSystem;
    // Need to use this bool and check in update to avoid a unity error. 
    private bool _pointerOverUI = false;

    private int _firstClickedObjectID;

    private GameObject _currentPCInstance; 

    private void Start()
    {
        _eventSystem = EventSystem.current;

        // Click on PC icon. 
        PCInfo.OnSelectPC += HandleClick;

        // Click on PC instance. 
        S.I.IM.PC.Home.SelectOrCenter.performed += CheckIfPCClicked;
        S.I.IM.PC.Scavenge.Select.performed += CheckIfPCClicked;

        /*SelectedIdleSubstate.OnPCDeselected*/
        SelectedIdleSubstate.OnDeselectPC += () => { Debug.Log("OnDeselectPC called by SelectedIdleSubstate"); _currentPCInstance = null; };

        PlayerIdleState.OnPCDeselected += () => ChangePC(null);
    }

    private void Update()
    {
        _pointerOverUI = _eventSystem.IsPointerOverGameObject();
    }

    private void OnDisable()
    {
        // Click on PC icon. 
        PCInfo.OnSelectPC -= HandleClick;

        // Click on PC instance. 
        S.I.IM.PC.Home.SelectOrCenter.performed -= CheckIfPCClicked;
        S.I.IM.PC.Scavenge.Select.performed -= CheckIfPCClicked;

        /*SelectedIdleSubstate.OnPCDeselected*/
        SelectedIdleSubstate.OnDeselectPC -= () => { Debug.Log("OnDeselectPC called by SelectedIdleSubstate"); _currentPCInstance = null; };
   
        PlayerIdleState.OnPCDeselected -= () => ChangePC(null);
    }

    private void CheckIfPCClicked(InputAction.CallbackContext context)
    {
        if (!_pointerOverUI)
        {
            // Only raycast to PC layer. 
            RaycastHit[] hits = Physics.RaycastAll(
                Camera.main.ScreenPointToRay(S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()),
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
            // If the second click was on the same PC as first click, center on that PC. 
            if (pcInstance.GetInstanceID() == _firstClickedObjectID)
            {
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
            // Select PC. 
            ChangePC(pcInstance); 

            // Get reference to first object clicked, to compare on second click. 
            _firstClickedObjectID = pcInstance.GetInstanceID(); 
        }

        _lastClickTime = currentClickTime;
    }

    private void ChangePC(GameObject clickedPCInstance)
    {
        // If clicked PC is NOT your current PC (true also when _currentPCInstance == null), 
        if (_currentPCInstance != clickedPCInstance)
        {
            // If there is a currently selected PC, set its Selected to false. 
            if (_currentPCInstance)
            {
                _currentPCInstance.GetComponent<PlayerController>().SetSelected(false);
            }

            // Set clicked PC's Selected to true.
            if (clickedPCInstance != null)
            {
                clickedPCInstance.GetComponent<PlayerController>().SetSelected(true);
            }
         
            _currentPCInstance = clickedPCInstance; 
        }
    }
}