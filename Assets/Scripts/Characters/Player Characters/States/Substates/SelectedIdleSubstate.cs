using UnityEngine;
using UnityEngine.InputSystem;

public class SelectedIdleSubstate : MonoBehaviour
{
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

    // Right click deselects PC while not doing anything. 
    private void Deselect(InputAction.CallbackContext context)
    {
        // Activate NotSelected substate. 
        transform.parent.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(true);

        // Deactivate selected state game object. 
        gameObject.SetActive(false);
    }
}