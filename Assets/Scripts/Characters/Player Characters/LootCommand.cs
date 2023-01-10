using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LootCommand : MonoBehaviour
{
    [SerializeField]
    private LayerMask lootContainerLayer;

    private Transform currentlySelectedPC;

    public static event Action<Transform, Transform> onClickedLoot;

    private void Start()
    {
        S.I.IM.PC.Scavenge.Select.performed += LeftClick;
        PlayerCharacterController.onChangedSelectedCharacter += ChangeCurrentCharacter;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Scavenge.Select.performed -= LeftClick;
        PlayerCharacterController.onChangedSelectedCharacter -= ChangeCurrentCharacter;
    }

    private void LeftClick(InputAction.CallbackContext context)
    {
        if (currentlySelectedPC != null)
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(
                S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()), 
                100, 
                lootContainerLayer);

            // If click hit anything at all
            if (hits.Length > 0)
            {
                // Find the closest loot container to the camera
                RaycastHit closestHit = hits[0];

                foreach (RaycastHit hit in hits)
                {
                    if (Vector3.Distance(transform.position, hit.transform.position)
                        < Vector3.Distance(transform.position, closestHit.transform.position))
                    {
                        closestHit = hit;
                    }
                }

                //Debug.Log(currentlySelectedPC.name + " clicked on loot container: "
                //    + closestHit.collider.transform.name);

                // Send signal to LootAction telling them to start looting closest container
                onClickedLoot?.Invoke(currentlySelectedPC, closestHit.collider.transform);
            }
        }
    }

    private void ChangeCurrentCharacter(Transform newCharacterTransform)
    {
        if (newCharacterTransform.CompareTag("MainCamera"))
        {
            currentlySelectedPC = null;
        }
        else
        {
            currentlySelectedPC = newCharacterTransform;
        }
    }
}