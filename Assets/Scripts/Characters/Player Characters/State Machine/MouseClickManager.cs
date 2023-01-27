using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// TODO: Turn this into "Selected" State? Then won't need the SelectedPCSO? 
// Sends raycast from mouse position into game world. Sets state of active PC based on what was clicked (ground, loot, enemy, etc.) 
public class MouseClickManager : MonoBehaviour
{
   // public static event Action<Transform, Transform> OnClickedLoot;

    [SerializeField]
	private SelectedPCSO _selectedPCSO;

    [SerializeField, Header("Individual Layers")]
    private LayerMask _pCLayer;
	[SerializeField]
    private LayerMask _enemyLayer;
	[SerializeField]
    private LayerMask _lootContainerLayer;
	[SerializeField]
    private LayerMask _groundLayer;

    private void Start()
    {
        // started is single or double click, canceled is single click only. 
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/started += HandleClick;
        S.I.IM.PC.Scavenge.Select.performed += HandleClick;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/started -= HandleClick;
        S.I.IM.PC.Scavenge.Select.performed -= HandleClick;
    }

    private void HandleClick(InputAction.CallbackContext context)
    {
        if (_selectedPCSO.PCSO != null)
        {
            // RaycastAll to see what was hit. 
            RaycastHit[] hits = Physics.RaycastAll(
                Camera.main.ScreenPointToRay(S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()), 
                1000);

            // If raycast hits anything, 
            if (hits.Length > 0)
            {
                // Early return if raycast hits PC (PCSelector handles these clicks). 
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.layer == _pCLayer)
                    {
                        return;
                    }
                }

                // If no PC was hit by raycast, 
                // Check for enemy clicks first. 
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.layer == _enemyLayer)
                    {
                        // Set fighting variables here. 

                        // Return so that multiple hits don't get called. 
                        return;
                    }
                }

                // Check for loot clicks second. 
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.layer == _lootContainerLayer)
                    {
                        // Set looting variables here.
                        Transform states = _selectedPCSO.PCSO.PCInstance.transform.GetChild(4); 

                        // Deactivate all current states. 
                        foreach (Transform state in states)
                        {
                            state.gameObject.SetActive(false);
                        }

                        // Activate and set variables for RunToLootState. 
                        RunToLootState runToLootState = states.gameObject.GetComponent<RunToLootState>();
                        runToLootState.gameObject.SetActive(true);
                        runToLootState.LootContainerTransform = hit.transform;

                        // Do this in RunToLootState's OnEnable instead. 
                        // Set new destination for PC's NavMeshAgent. 
                        //_selectedPCSO.PCSO.PCInstance.GetComponent<NavMeshAgent>().destination = hit.transform.GetChild(0).transform.position;

                        // Return so that multiple hits don't get called. 
                        return;
                    }
                }

                // Check for ground clicks last. 
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.layer == _groundLayer)
                    {
                        // Set movement variables here.
                        // Set new destination for PC's NavMeshAgent. 
                        _selectedPCSO.PCSO.PCInstance.GetComponent<NavMeshAgent>().destination = hit.point;

                        // Return so that multiple hits don't get called. 
                        return;
                    }
                }
            }
        }
    }
}