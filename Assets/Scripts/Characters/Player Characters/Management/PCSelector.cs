using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCSelector : MonoBehaviour
{
    public static event Action<Transform> OnDoubleClickPCButton;

    [SerializeField]
    private PCSOListSO availablePCsSO;

/*    [SerializeField]
    private SelectedPCSO _selectedPCSO;*/

    [SerializeField]
    private LayerMask _playerCharacterLayer;

    [SerializeField]
    private float _doubleClickTimeLimit = 0.5f;
    private float _lastClickTime = 0f;

    private void Start()
    {
        PCItemSO.OnSelectPC += HandleButtonClick;

        S.I.IM.PC.Home.SelectOrCenter.started += Select;
        S.I.IM.PC.Scavenge.Select.performed += Select;
/*        S.I.IM.PC.Home.Deselect.performed += Deselect;
        S.I.IM.PC.Scavenge.Deselect.performed += Deselect;*/

        // Trying double click stuff
/*        S.I.IM.PC.Home.SelectOrCenter.performed += Performed;
        S.I.IM.PC.Home.SelectOrCenter.canceled += Canceled;
        S.I.IM.PC.Home.SelectOrCenter.started += Started;*/
    }

    private void OnDisable()
    {
        PCItemSO.OnSelectPC -= HandleButtonClick;

        S.I.IM.PC.Home.SelectOrCenter.started -= Select;
        S.I.IM.PC.Scavenge.Select.performed -= Select;
/*        S.I.IM.PC.Home.Deselect.performed -= Deselect;
        S.I.IM.PC.Scavenge.Deselect.performed -= Deselect;*/

        // Trying double click stuff
/*        S.I.IM.PC.Home.SelectOrCenter.performed -= Performed;
        S.I.IM.PC.Home.SelectOrCenter.canceled -= Canceled;
        S.I.IM.PC.Home.SelectOrCenter.started -= Started;*/
    }

/*    private void Performed(InputAction.CallbackContext context)
    {
        Debug.Log("Performed: Double Clicked");
    }
    private void Canceled(InputAction.CallbackContext context)
    {
        Debug.Log("Canceled: Single Clicked");
    }
    private void Started(InputAction.CallbackContext context)
    {
        Debug.Log("Started: Single or Double Clicked");
    }*/

    private void Select(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        // Only checks for collisions on PlayerCharacter Layer
        if (Physics.Raycast(Camera.main.ScreenPointToRay(
                S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()),
                out hit,
                1000,
                _playerCharacterLayer))
        {
            ChangePC(GetSOFromInstance(hit.collider.gameObject));

            //TODO: Redo this using the instance from the PCSO after setting up scene PC instantiation.
            // Deactivate old selected PC's "selected" icon
/*            if (_selectedPCSO.PCSO != null)
            {
                _selectedPCSO.PCSO.PCInstance.GetComponentInChildren<SelectedPCIcon>().DeactivateIcon();
            }

            // TODO: Change this to actually change the SO, not the instance variable.
            // Set new selected PC in SO
            _selectedPCSO.PCSO.PCInstance = hit.collider.gameObject;

            // Activate new selected PC's "selected" icon
            _selectedPCSO.PCSO.PCInstance.GetComponentInChildren<SelectedPCIcon>().ActivateIcon();*/
        }
    }

    private PCItemSO GetSOFromInstance(GameObject instance)
    {
        foreach (PCItemSO pCItemSO in availablePCsSO.PCItemSOs)
        {
            if (pCItemSO.PCInstance.GetInstanceID() == instance.GetInstanceID())
            {
                return pCItemSO;
            }
        }
        return null;
    }

    // Still get double click if you click on two different buttons within time limit?
    private void HandleButtonClick(PCItemSO pCItemSO)
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
        // Deactivate "selected" substate of current PC in SelectedSubstate class. 

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

/*        // Deactivate old selected PC's "selected" icon
        if (_selectedPCSO.PCSO != null)
        {
            _selectedPCSO.PCSO.PCInstance.GetComponentInChildren<SelectedPCIcon>().DeactivateIcon();
        }

        // Set new selected PCSO
        _selectedPCSO.PCSO = newPCItemSO;

        // Activate new selected PC's "selected" icon
        _selectedPCSO.PCSO.PCInstance.GetComponentInChildren<SelectedPCIcon>().ActivateIcon();*/
    }

    // TODO: Call this from Idle state instead. Want right click to cancel actions in other states. 
    // OR, check if player is in idle state. 
/*    private void Deselect(InputAction.CallbackContext context)
    {
        if (_selectedPCSO.PCSO != null)
        {
            // Deactivate old currentAgent's "selected" icon
            _selectedPCSO.PCSO.PCInstance.GetComponentInChildren<SelectedPCIcon>().DeactivateIcon();

            // Set selected PC to null
            _selectedPCSO.PCSO = null;
        }
    }*/
}