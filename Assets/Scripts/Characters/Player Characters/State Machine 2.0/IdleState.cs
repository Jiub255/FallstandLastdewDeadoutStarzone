using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour
{
    [SerializeField]


    private void Update()
    {
        // Check for enemies. 


        // Check for loot containers. 
        

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