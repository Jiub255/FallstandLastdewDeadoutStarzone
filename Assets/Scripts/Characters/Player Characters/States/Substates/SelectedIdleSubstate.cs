using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectedIdleSubstate : MonoBehaviour
{
    public static event Action OnDeselectPC;

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

    // Problem with deselecting and not being able to reselect might be here. 
    // Right click deselects PC while not doing anything. 
    private void Deselect(InputAction.CallbackContext context)
    {
        // Activate NotSelected substate. 
        transform.parent.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(true);

        // PCSelector and Transparentizer listen for this, sets _currentPC to null. 
        OnDeselectPC?.Invoke();

        // Deactivate Selected substate. 
        gameObject.SetActive(false);
    }
}