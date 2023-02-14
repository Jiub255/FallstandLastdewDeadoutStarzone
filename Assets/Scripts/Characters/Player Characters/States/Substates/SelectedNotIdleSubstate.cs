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
        // Activate Idle state (and selected substate?). 
        _idleState.SetActive(true);
        _idleState.transform.GetChild(0).gameObject.SetActive(true);

        // Deactivate current state. 
        transform.parent.gameObject.SetActive(false);
    }
}