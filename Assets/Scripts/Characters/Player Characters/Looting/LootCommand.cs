using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LootCommand : MonoBehaviour
{
    public static event Action<Transform, Transform> OnClickedLoot;

    [SerializeField]
    private LayerMask _lootContainerLayer;

    [SerializeField]
    private SelectedPCSO _selectedPCSO;

    private void Start()
    {
        S.I.IM.PC.Scavenge.Select.performed += Loot;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Scavenge.Select.performed -= Loot;
    }

    private void Loot(InputAction.CallbackContext context)
    {
        Debug.Log("Loot Performed");

        if (_selectedPCSO.PCSO.PCInstance != null)
        {
        Debug.Log("PCInstance != null");
            // Raycast checks for loot containers where the mouse clicked.
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(
                S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()), 
                1000, 
                _lootContainerLayer);

            // If click hit anything at all
            if (hits.Length > 0)
            {
                // Find the closest loot container to the camera
                RaycastHit closestHit = hits[0];

                foreach (RaycastHit hit in hits)
                {
                    Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

                    if (Vector3.Distance(transform.position, hit.transform.position)
                        < Vector3.Distance(transform.position, closestHit.transform.position))
                    {
                        closestHit = hit;
                    }
                }

                // Send signal to LootAction telling them to start looting closest container
                OnClickedLoot?.Invoke(_selectedPCSO.PCSO.PCInstance.transform, closestHit.collider.transform);
            }
        }
    }
}