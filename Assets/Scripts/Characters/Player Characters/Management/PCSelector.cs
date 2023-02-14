using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PCSelector : MonoBehaviour
{
    public static event Action<Transform> OnDoubleClickPCButton;

    [SerializeField]
    private PCSOListSO _availablePCsSO;

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

        PCItemSO.OnSelectPC += HandlePCIconClick;

        S.I.IM.PC.Home.SelectOrCenter.started += Select;
        S.I.IM.PC.Scavenge.Select.performed += Select;
    }

    private void OnDisable()
    {
        PCItemSO.OnSelectPC -= HandlePCIconClick;

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
                ChangePC(GetSOFromInstance(hits[0].collider.gameObject));
            }
        }
    }

    private PCItemSO GetSOFromInstance(GameObject instance)
    {
        foreach (PCItemSO pCItemSO in _availablePCsSO.PCItemSOs)
        {
            if (pCItemSO.PCInstance.GetInstanceID() == instance.GetInstanceID())
            {
                return pCItemSO;
            }
        }
        return null;
    }

    // Still get double click if you click on two different buttons within time limit?
    private void HandlePCIconClick(PCItemSO pCItemSO)
    {
        float currentClickTime = Time.realtimeSinceStartup;

        if ((currentClickTime - _lastClickTime) < _doubleClickTimeLimit)
        {
            // Double click: Center camera on PC. Use event?
            OnDoubleClickPCButton?.Invoke(pCItemSO.PCInstance.transform);
        }
        else
        {
            // Single click: Select PC.
            ChangePC(pCItemSO);
        }

        _lastClickTime = currentClickTime;
    }

    private void ChangePC(PCItemSO newPCItemSO)
    {
        // "Selected" substate of current PC gets deactivated in SelectedSubstate class. 

        Transform states = newPCItemSO.PCInstance.transform.GetChild(4);

        foreach (Transform state in states)
        {
            // If state is currently active, 
            if (state.gameObject.activeInHierarchy)
            {
                // Activate "selected" substate. 
                state.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}