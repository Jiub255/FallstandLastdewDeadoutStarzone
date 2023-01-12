using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// Put this on Game Controller
public class PCMovement : MonoBehaviour
{
    public static event Action<InputAction.CallbackContext> OnMove;

    [SerializeField]
    private LayerMask _transparentableAndPCLayers;

    [SerializeField]
    private SelectedPCSO _selectedPCSO;

    private void Start()
    {
        S.I.IM.PC.Home.Select.performed += Move;
        S.I.IM.PC.Scavenge.Select.performed += Move;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.Select.performed -= Move;
        S.I.IM.PC.Scavenge.Select.performed -= Move;
    }

    private void Move(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        // Ignore "Transparentable" and "PlayerController" layers.
        if (_selectedPCSO.SelectedPCGO != null &&
            Physics.Raycast(Camera.main.ScreenPointToRay(
                S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()), 
                out hit, 
                100, 
                ~_transparentableAndPCLayers))
        {
            OnMove?.Invoke(context);

            _selectedPCSO.SelectedPCGO.GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}