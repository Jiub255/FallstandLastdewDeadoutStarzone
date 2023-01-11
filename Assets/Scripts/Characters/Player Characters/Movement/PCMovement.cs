using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// Put this on Game Controller
public class PCMovement : MonoBehaviour
{
    [SerializeField]
    private LayerMask transparentableAndPCLayers;

    [SerializeField]
    private SelectedPCSO selectedPCSO;

    public static event Action<InputAction.CallbackContext> onMove;

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
        if (selectedPCSO.selectedPCGO != null &&
            Physics.Raycast(Camera.main.ScreenPointToRay(
                S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()), 
                out hit, 
                100, 
                ~transparentableAndPCLayers))
        {
            onMove?.Invoke(context);

            selectedPCSO.selectedPCGO.GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
}