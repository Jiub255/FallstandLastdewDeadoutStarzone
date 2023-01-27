using System.Collections;
using System.Collections.Generic;
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

    private void Deselect(InputAction.CallbackContext context)
    {
        // Deactivate selected state game object. 
        gameObject.SetActive(false);
    }
}