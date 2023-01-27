using UnityEngine;
using UnityEngine.InputSystem;

public class SelectedNotIdleSubstate : MonoBehaviour
{
	[SerializeField]
	private GameObject _idleState;

    private void OnEnable()
    {
        S.I.IM.PC.Home.Deselect.performed += Deselect;
        S.I.IM.PC.Scavenge.Deselect.performed += Deselect;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.Deselect.performed -= Deselect;
        S.I.IM.PC.Scavenge.Deselect.performed -= Deselect;
    }

    private void Deselect(InputAction.CallbackContext context)
    {
        // Deactivate current state. 
        transform.parent.gameObject.SetActive(false);

        // Activate Idle state (and selected substate?). 
        _idleState.SetActive(true);
        _idleState.transform.GetChild(0).gameObject.SetActive(true);
    }
}