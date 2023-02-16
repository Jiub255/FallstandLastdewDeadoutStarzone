using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PCSelector : MonoBehaviour
{
    public static event Action<Transform> OnDoubleClickPCButton;

    [SerializeField]
    private GOListSO _pcInstancesSO;

    [SerializeField]
    private LayerMask _pcLayerMask;

    [SerializeField]
    private float _doubleClickTimeLimit = 0.5f;
    private float _lastClickTime = 0f;

    private EventSystem _eventSystem;
    private bool _pointerOverUI = false;

    private void Start()
    {
        _eventSystem = EventSystem.current;

        // Click on PC icon. 
        PCInfo.OnSelectPC += HandlePCIconClick;

        // Click on PC instance. 
        S.I.IM.PC.Home.SelectOrCenter.started += Select;
        S.I.IM.PC.Scavenge.Select.performed += Select;
    }

    private void OnDisable()
    {
        // Click on PC icon. 
        PCInfo.OnSelectPC -= HandlePCIconClick;

        // Click on PC instance. 
        S.I.IM.PC.Home.SelectOrCenter.started -= Select;
        S.I.IM.PC.Scavenge.Select.performed -= Select;
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

    private void Select(InputAction.CallbackContext context)
    {
        if (!_pointerOverUI)
        {
            // Only raycast to PC layer. 
            RaycastHit[] hits = Physics.RaycastAll(
                Camera.main.ScreenPointToRay(S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()),
                1000,
                _pcLayerMask);

            if (hits.Length > 0)
            {
                ChangePC(hits[0].collider.gameObject);
            }
        }
    }

    // Still get double click if you click on two different buttons within time limit?
    private void HandlePCIconClick(GameObject pcInstance)
    {
        float currentClickTime = Time.realtimeSinceStartup;

        if ((currentClickTime - _lastClickTime) < _doubleClickTimeLimit)
        {
            // Double click: Center camera on PC. Use event?
            OnDoubleClickPCButton?.Invoke(pcInstance.transform);
        }
        else
        {
            // Single click: Select PC.
            ChangePC(pcInstance);
        }

        _lastClickTime = currentClickTime;
    }

    private void ChangePC(GameObject newPCInstance)
    {
        // "Selected" substate of current PC gets deactivated in SelectedSubstate class. 

        Transform states = newPCInstance.GetComponentInChildren<States>().transform;

        foreach (Transform state in states)
        {
            // If state is currently active, 
            if (state.gameObject.activeInHierarchy)
            {
                // Activate SelectedSubstate. 
                state.GetChild(0).gameObject.SetActive(true);

                // Deactivate NotSelectedSubstate.
                state.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}