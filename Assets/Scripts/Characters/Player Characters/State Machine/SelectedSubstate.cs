using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectedSubstate : MonoBehaviour
{
    [SerializeField, Header("Individual Layers")]
    private LayerMask _pCLayer;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _lootContainerLayer;
    [SerializeField]
    private LayerMask _groundLayer;

    private void OnEnable()
    {
        // started is single or double click, canceled is single click only. 
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/started += HandleClick;
        S.I.IM.PC.Scavenge.Select.performed += HandleClick;

        // Activate selected icon. 
        transform.parent.parent.parent.GetComponentInChildren<SelectedPCIcon>().ActivateIcon();
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/started -= HandleClick;
        S.I.IM.PC.Scavenge.Select.performed -= HandleClick;

        // Deactivate selected icon. 
        transform.parent.parent.parent.GetComponentInChildren<SelectedPCIcon>().DeactivateIcon();
    }

    private void HandleClick(InputAction.CallbackContext context)
    {
        Debug.Log("Clicked");

        Transform states = transform.parent.parent;

        // RaycastAll to see what was hit. 
        RaycastHit[] hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()),
            1000);

        // If raycast hits anything, and mouse is not over UI, 
        if (hits.Length > 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log($"Hit {hits.Length} things");
            foreach (RaycastHit hit in hits)
            {
                Debug.Log($"{ hit.collider.name} on {LayerMask.LayerToName(hit.collider.gameObject.layer)} layer");
            }

            // If a different was PC hit, deactivate "selected" substate (PCSelector handles activating new PC's substate). 
            foreach (RaycastHit hit in hits)
            {
                if ((_pCLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    Debug.Log("Hit PC: " + hit.collider.name);
                    
                    // If PC is not currently selected PC, 
                    if (hit.collider.gameObject.GetInstanceID() != transform.parent.parent.parent.gameObject.GetInstanceID())
                    {
                        // Deactivate "selected" substate. 
                        gameObject.SetActive(false);
                    }

                    return;
                }
            }

            // If no PC was hit by raycast, 
            // Check for enemy clicks first. 
            foreach (RaycastHit hit in hits)
            {
                if ((_enemyLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    Debug.Log("Hit enemy: " + hit.collider.name);
                    
                    // Set fighting variables here. 

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }

            // Check for loot clicks second. 
            foreach (RaycastHit hit in hits)
            {
                Debug.Log(_lootContainerLayer & (1 << hit.collider.gameObject.layer));
                if ((_lootContainerLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    Debug.Log("Hit loot container: " + hit.collider.name);
                    
                    // Set looting variables here.

                    // Deactivate current state (if not in run to loot state already). 
                    if (transform.parent.name != "Run To Loot")
                    {
                        transform.parent.gameObject.SetActive(false);
                    }

                    // Activate and set variables for RunToLootState. 
                    RunToLootState runToLootState = states.gameObject.GetComponentInChildren<RunToLootState>(true);
                    runToLootState.LootContainerTransform = hit.transform.parent;
                    runToLootState.gameObject.SetActive(true);
                    runToLootState.transform.GetChild(0).gameObject.SetActive(true);

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }

            // Check for ground clicks last. 
            foreach (RaycastHit hit in hits)
            {
                if ((_groundLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    Debug.Log("Hit ground at: " + hit.point);
                    // Set movement variables here.
                    // Set new destination for PC's NavMeshAgent. 
                    transform.parent.parent.parent.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = hit.point;

                    // Deactivate current state (if not in run state already). 
                    if (transform.parent.name != "Run")
                    {
                        transform.parent.gameObject.SetActive(false);
                    }

                    // Activate Run state and selected substate. 
                    RunState runState = states.gameObject.GetComponentInChildren<RunState>(true);
                    runState.gameObject.SetActive(true);
                    runState.transform.GetChild(0).gameObject.SetActive(true);

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }
        }
    }
}