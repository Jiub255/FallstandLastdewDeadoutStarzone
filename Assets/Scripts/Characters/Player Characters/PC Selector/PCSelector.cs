using UnityEngine;
using UnityEngine.InputSystem;

public class PCSelector : MonoBehaviour
{
    [SerializeField]
    private SelectedPCSO selectedPCSO;

    [SerializeField]
    private LayerMask playerCharacterLayer;

    private void Start()
    {
        S.I.IM.PC.Home.Select.performed += Select;
        S.I.IM.PC.Scavenge.Select.performed += Select;
        S.I.IM.PC.Home.Deselect.performed += Deselect;
        S.I.IM.PC.Scavenge.Deselect.performed += Deselect;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.Select.performed -= Select;
        S.I.IM.PC.Scavenge.Select.performed -= Select;
        S.I.IM.PC.Home.Deselect.performed -= Deselect;
        S.I.IM.PC.Scavenge.Deselect.performed -= Deselect;
    }

    private void Select(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        // Only checks for collisions on PlayerCharacter Layer
        if (Physics.Raycast(Camera.main.ScreenPointToRay(
                S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()),
                out hit,
                100,
                playerCharacterLayer))
        {
            // Deactivate old selected PC's "selected" icon
            if (selectedPCSO.selectedPCGO != null)
            {
                selectedPCSO.selectedPCGO.GetComponentInChildren<SelectedIcon>().DeactivateIcon();
            }

            // Set new selected PC in SO
            selectedPCSO.selectedPCGO = hit.collider.gameObject;

            // Activate new selected PC's "selected" icon
            selectedPCSO.selectedPCGO.GetComponentInChildren<SelectedIcon>().ActivateIcon();
        }
    }

    private void Deselect(InputAction.CallbackContext context)
    {
        if (selectedPCSO.selectedPCGO != null)
        {
            // Deactivate old currentAgent's "selected" icon
            selectedPCSO.selectedPCGO.GetComponentInChildren<SelectedIcon>().DeactivateIcon();

            // Set selected PC to null
            selectedPCSO.selectedPCGO = null;
        }
    }
}