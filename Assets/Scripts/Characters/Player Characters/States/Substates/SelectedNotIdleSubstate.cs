using UnityEngine;
using UnityEngine.InputSystem;

public class SelectedNotIdleSubstate : MonoBehaviour
{
	[SerializeField] 
	private GameObject _idleState; 

    private void OnEnable() 
    { 
        S.I.IM.PC.Home.Deselect.performed += CancelAction; 
        S.I.IM.PC.Scavenge.Deselect.performed += CancelAction; 
    }

    private void OnDisable() 
    {
        S.I.IM.PC.Home.Deselect.performed -= CancelAction; 
        S.I.IM.PC.Scavenge.Deselect.performed -= CancelAction; 
    }

    // Switch to idle selected state when right clicking while doing something. 
    private void CancelAction(InputAction.CallbackContext context)
    {
        // Activate Idle state. 
        _idleState.SetActive(true); 
        // Activate Selected substate. 
        _idleState.transform.GetComponentInChildren<SelectedSubstate>(true).gameObject.SetActive(true); 
        // Deactivate NotSelected substate. 
        _idleState.transform.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(false); 
        // Deactivate current state. 
        transform.parent.gameObject.SetActive(false); 
    }
}