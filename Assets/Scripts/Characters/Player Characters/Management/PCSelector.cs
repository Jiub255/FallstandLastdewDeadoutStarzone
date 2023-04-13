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
    private bool _pointerOverUI = false;

    private int _firstClickedObjectID;

    private GameObject _currentPCInstance; 

    private void Start()
    {
        _eventSystem = EventSystem.current;

        // Just so GetInstanceID works. 
        //_currentPC = new GameObject("NotNull");

        // Click on PC icon. 
        PCInfo.OnSelectPC += HandleClick;

        // Click on PC instance. 
        S.I.IM.PC.Home.SelectOrCenter.performed += CheckIfPCClicked;
        S.I.IM.PC.Scavenge.Select.performed += CheckIfPCClicked;

        /*SelectedIdleSubstate.OnPCDeselected*/
        NotSelectedSubstate.OnDeselectPC += () => { Debug.Log("OnDeselectPC called by NotSelectedSubstate"); _currentPCInstance = null; };
    }

    private void OnDisable()
    {
        // Click on PC icon. 
        PCInfo.OnSelectPC -= HandleClick;

        // Click on PC instance. 
        S.I.IM.PC.Home.SelectOrCenter.performed -= CheckIfPCClicked;
        S.I.IM.PC.Scavenge.Select.performed -= CheckIfPCClicked;

        /*SelectedIdleSubstate.OnPCDeselected*/
        NotSelectedSubstate.OnDeselectPC -= () => { Debug.Log("OnDeselectPC called by NotSelectedSubstate"); _currentPCInstance = null; };
    }

    private void Update()
    {
        if (_eventSystem.IsPointerOverGameObject())
        {
            _pointerOverUI = true;
        }
        else
        {
            _pointerOverUI = false;
        }
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

            // If there were any hits, they must have been PC's. 
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    Debug.Log($"Raycast hit {hit.collider.name}");
                }

                // This handles double/single clicking. 
                HandleClick(hits[0].transform.parent.gameObject);
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
        // "Selected" substate of current PC gets deactivated in SelectedSubstate class. 
        // TODO - Previous PC not getting deselected when selecting a new one, but sometimes not. 

        // If clicked PC is NOT your current PC (true also when _currentPCInstance == null), 
        if (_currentPCInstance != clickedPCInstance)
        {
            // If there is a currently selected PC, set it to not selected substate of whatever state it's in. 
            if (_currentPCInstance)
            {
                Transform currentPCStates = _currentPCInstance.GetComponentInChildren<States>().transform;

                foreach (Transform state in currentPCStates)
                {
                    // If state is currently active, 
                    if (state.gameObject.activeInHierarchy)
                    {
                        Debug.Log($"{state.name} is the active state of current pc {_currentPCInstance.name}");

                        // Activate NotSelectedSubstate.
                        state.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(true);

                        // Deactivate SelectedSubstate. 
                        state.GetComponentInChildren<SelectedSubstate>(true).gameObject.SetActive(false);

                        // Hopefully shouldn't need this break since there should never be two states active at the same time. 
                        break;
                    }

                    // Is this necessary? Is it harmful (because of OnEnable/OnDisable stuff)?  
                    // Was GetChild(0) supposed to be the idle state? 
                    // And really, it should never get here, because every PC should have one state active at all times. 
                    // TODO - Figure this nonsense out. 
                   // currentPCStates.GetChild(0).GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(true);
                   // currentPCStates.GetChild(0).GetComponentInChildren<SelectedSubstate>(true).gameObject.SetActive(false);
                }
            }

            // Set clicked PC to selected substate of whatever state it's in. 
            Transform newPCStates = clickedPCInstance.GetComponentInChildren<States>().transform;
            foreach (Transform state in newPCStates)
            {
                // If state is currently active, 
                if (state.gameObject.activeInHierarchy)
                {
                    // Activate SelectedSubstate. 
                    state.GetComponentInChildren<SelectedSubstate>(true).gameObject.SetActive(true);

                    // Deactivate NotSelectedSubstate.
                    state.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(false);

                    break;
                }

                // Is this necessary? Is it harmful (because of OnEnable/OnDisable stuff)?  
                // Was GetChild(0) supposed to be the idle state? 
                // And really, it should never get here, because every PC should have one state active at all times. 
                // TODO - Figure this nonsense out. 
                // newPCStates.GetChild(0).GetComponentInChildren<SelectedSubstate>(true).gameObject.SetActive(true);
                // newPCStates.GetChild(0).GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(false);
            }
        }

        _currentPCInstance = clickedPCInstance; 
    }
}