using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectedSubstate : MonoBehaviour
{
    public static event Action<Transform> OnSelectPC;
    public static event Action OnDeselectPC;

    [SerializeField, Header("Individual Layers")]
    private LayerMask _pCLayerMask;
    [SerializeField]
    private LayerMask _enemyLayerMask;
    [SerializeField]
    private LayerMask _lootContainerLayerMask;
    [SerializeField]
    private LayerMask _groundLayerMask;

    private bool _pointerOverUI = false;

    private EventSystem _eventSystem;

    private void OnEnable()
    {
        // Cache EventSystem.current since we'll be checking it every frame. 
        _eventSystem = EventSystem.current;

        // started is single or double click, canceled is single click only. 
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/started += HandleClick;
        S.I.IM.PC.Scavenge.Select.performed += HandleClick;

        // Activate selected icon. 
        transform.parent.parent.parent.GetComponentInChildren<SelectedPCIcon>().ActivateIcon();

        // Send Transparentizer PC's transform to set as currently selected. 
        OnSelectPC?.Invoke(transform);
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/started -= HandleClick;
        S.I.IM.PC.Scavenge.Select.performed -= HandleClick;

        // Send Transparentizer signal to set current PC transform to null. 
        OnDeselectPC?.Invoke();
    }

    private void Update()
    {
        if (_eventSystem.IsPointerOverGameObject())
        {
            _pointerOverUI = true;
        }
        else
        {
            _pointerOverUI = false;
        }
    }

    private void HandleClick(InputAction.CallbackContext context)
    {
        Transform states = transform.parent.parent;

        // RaycastAll to see what was hit. 
        RaycastHit[] hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(S.I.IM.PC.World.MousePosition.ReadValue<Vector2>()),
            1000);

        // If raycast hits anything, and mouse is not over UI, 
        if (hits.Length > 0 && !_pointerOverUI)
        {
            // Check to see if raycast hit a PC first. 
            foreach (RaycastHit hit in hits)
            {
                //Using "LayerMask.Contains()" extension method instead of writing "if ((_pCLayer & (1 << hit.collider.gameObject.layer)) != 0)" each time. 
                if (_pCLayerMask.Contains(hit.collider.gameObject.layer))
                {
                    // If PC is not currently selected PC, 
                    if (hit.transform.parent.GetInstanceID() != transform.parent.parent.parent.GetInstanceID())
                    {
                        // Activate NotSelectedSubstate. 
                        transform.parent.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(true);
                        // Deactivate SelectedSubstate (PCSelector handles activating new PC's substate). 
                        gameObject.SetActive(false);
                    }

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }

            // Check for enemy clicks second. 
            foreach (RaycastHit hit in hits)
            {
                if (_enemyLayerMask.Contains(hit.transform.parent.gameObject.layer))
                {
                    RunToEnemyState runToEnemyState = states.gameObject.GetComponentInChildren<RunToEnemyState>(true);

                    // Make sure to get the right parent for the transform. 
                    // Set fighting variables. 
                    runToEnemyState.Target = hit.transform.parent;

                    // Switch current state (if not in run to enemy state already). 
                    //if (transform.parent.name != "Run To Enemy")
                    if (transform.parent.GetInstanceID() != runToEnemyState.GetInstanceID())
                    {
                        SwitchToState(runToEnemyState.gameObject);
                    }

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }

            // Check for loot clicks third. 
            foreach (RaycastHit hit in hits)
            {
                if (_lootContainerLayerMask.Contains(hit.collider.gameObject.layer))
                {
                    // Make sure container hasn't been looted and isn't currently being looted. 
                    if (!hit.transform.GetComponent<LootContainer>().Looted &&
                        !hit.transform.GetComponent<LootContainer>().IsBeingLooted)
                    {
                        RunToLootState runToLootState = states.gameObject.GetComponentInChildren<RunToLootState>(true);
                    
                        // Set looting variables. 
                        runToLootState.LootContainerTransform = hit.transform.parent;

                        // Switch current state (if not in run to loot state already). 
                        if (transform.parent.GetInstanceID() != runToLootState.GetInstanceID())
                        {
                            SwitchToState(runToLootState.gameObject);
                        }

                        // Return so that multiple hits don't get called. 
                        return;
                    }
                }
            }

            // Check for ground clicks last. 
            foreach (RaycastHit hit in hits)
            {
                if (_groundLayerMask.Contains(hit.collider.gameObject.layer))
                {
                    RunState runState = states.gameObject.GetComponentInChildren<RunState>(true);

                    // Set movement variables here.
                    // Set new destination for PC's NavMeshAgent. 
                    transform.parent.parent.parent.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = hit.point;

                    // Switch current state (if not in run state already). 
                    if (transform.parent.GetInstanceID() != runState.GetInstanceID())
                    {
                        SwitchToState(runState.gameObject);
                    }

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }
        }
    }

    private void SwitchToState(GameObject state)
    {
        // Activate RunToLootState. 
        state.SetActive(true);
        // Activate its Selected substate. 
        state.GetComponentInChildren<SelectedSubstate>(true).gameObject.SetActive(true);
        // Deactivate its NotSelected substate. 
        state.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(false);

        // Deactivate current state. 
        transform.parent.gameObject.SetActive(false);
    }
}