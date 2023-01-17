using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Put this on Game Controller
public class PCMovement : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext> OnMove;

    [SerializeField]
    private LayerMask _groundAndPCLayers;

    [SerializeField]
    private SelectedPCSO _selectedPCSO;

    private void Start()
    {
        // started is single or double click, canceled is single click only. 
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/started += Move;
        S.I.IM.PC.Scavenge.Select.performed += Move;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/started -= Move;
        S.I.IM.PC.Scavenge.Select.performed -= Move;
    }

    private void Move(InputAction.CallbackContext context)
    {
        // Only raycast to ground and PC layers. 
        if (_selectedPCSO.PCSO != null)
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(
                S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()), 100,
                _groundAndPCLayers);

            if (hits.Length > 0)
            {
                // Return if raycast hit a PC, so you don't move old PC to newly selected PC's position while selecting new PC.
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("PlayerCharacter")))
                    {
                        return;
                    }
                }
                // If no PC hit, look for ground hits. (Should only ever be one)
                foreach (RaycastHit hit in hits)
                {
                    // IsPointerOverGameObject checks if mouse is over any UI (HUD) object.
                    if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        // LootAction calls ResetLootingState from this.
                        OnMove?.Invoke(context);

                        // Set new destination for PC's NavMeshAgent
                        _selectedPCSO.PCSO.PCInstance.GetComponent<NavMeshAgent>().destination = hit.point;

                        // Return after first "Ground" layer hit found, just in case there's more than 1. (There never should be though)
                        return;
                    }
                }
            }
        }
    }
}