using UnityEngine;
using UnityEngine.InputSystem;

public class PCSelector : MonoBehaviour
{
    // Keep availablePCsSO here?
    [SerializeField]
    private AvailablePCsSO availablePCsSO;

    [SerializeField]
    private SelectedPCSO _selectedPCSO;

    [SerializeField]
    private LayerMask _playerCharacterLayer;

    private void Start()
    {
        PCItemSO.OnSelectPC += ChangePC;

        S.I.IM.PC.Home.SelectOrCenter.started += Select;
        S.I.IM.PC.Scavenge.Select.performed += Select;
        S.I.IM.PC.Home.Deselect.performed += Deselect;
        S.I.IM.PC.Scavenge.Deselect.performed += Deselect;

        // Trying double click stuff
/*        S.I.IM.PC.Home.SelectOrCenter.performed += Performed;
        S.I.IM.PC.Home.SelectOrCenter.canceled += Canceled;
        S.I.IM.PC.Home.SelectOrCenter.started += Started;*/
    }

    private void OnDisable()
    {
        PCItemSO.OnSelectPC -= ChangePC;

        S.I.IM.PC.Home.SelectOrCenter.started -= Select;
        S.I.IM.PC.Scavenge.Select.performed -= Select;
        S.I.IM.PC.Home.Deselect.performed -= Deselect;
        S.I.IM.PC.Scavenge.Deselect.performed -= Deselect;

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
                100,
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

    private void ChangePC(PCItemSO newPCItemSO)
    {
        // Deactivate old selected PC's "selected" icon
        if (_selectedPCSO.PCSO != null)
        {
            _selectedPCSO.PCSO.PCInstance.GetComponentInChildren<SelectedPCIcon>().DeactivateIcon();
        }

        // Set new selected PCSO
        _selectedPCSO.PCSO = newPCItemSO;

        // Activate new selected PC's "selected" icon
        _selectedPCSO.PCSO.PCInstance.GetComponentInChildren<SelectedPCIcon>().ActivateIcon();
    }

    private void Deselect(InputAction.CallbackContext context)
    {
        if (_selectedPCSO.PCSO != null)
        {
            // Deactivate old currentAgent's "selected" icon
            _selectedPCSO.PCSO.PCInstance.GetComponentInChildren<SelectedPCIcon>().DeactivateIcon();

            // Set selected PC to null
            _selectedPCSO.PCSO = null;
        }
    }
}