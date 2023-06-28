using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectedSubstate : MonoBehaviour
{
    public static event Action<Transform> OnSelectPC;
    //public static event Action OnDeselectPC;

    [SerializeField, Header("Individual Layers")]
    private LayerMask _pCLayerMask;
    [SerializeField]
    private LayerMask _enemyLayerMask;
    [SerializeField]
    private LayerMask _lootContainerLayerMask;
    [SerializeField]
    private LayerMask _groundLayerMask;

    [SerializeField, Header("States")]
    private ApproachEnemyState _approachEnemyState;
    [SerializeField]
    private ApproachLootState _approachLootState;
    [SerializeField]
    private RunState _runState;

    private bool _pointerOverUI = false;
    private EventSystem _eventSystem;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private InputAction _mousePositionAction;

    private void Start/*OnEnable*/()
    {
        _navMeshAgent = transform.root.GetComponent<UnityEngine.AI.NavMeshAgent>();
        _mousePositionAction = S.I.IM.PC.World.MousePosition;

        // Cache EventSystem.current since it gets checked every frame. 
        _eventSystem = EventSystem.current;

        // started is single or double click, canceled is single click only. 
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/performed += HandleClick;
        S.I.IM.PC.Scavenge.Select.performed += HandleClick;

        // Activate selected icon. 
        transform.root.GetComponentInChildren<SelectedPCIcon>().ActivateIcon();

        // Send Transparentizer PC's transform to set as currently selected. 
        OnSelectPC?.Invoke(transform.root);
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/performed -= HandleClick;
        S.I.IM.PC.Scavenge.Select.performed -= HandleClick;

        // Send Transparentizer and PCSelector the signal to set current PC transform to null. 
        // TODO - This is causing problems because it sends this event whenever current PC switches states, 
        // which makes PCSelector set _currentPCInstance to null. 
        // Try putting it in NotSelectedSubstate's OnEnable instead?
       // OnDeselectPC?.Invoke();
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
        //Transform states = transform.parent.parent;

        // RaycastAll to see what was hit. 
        RaycastHit[] hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>()),
            1000);

        // If raycast hits anything, and mouse is not over UI, 
        if (hits.Length > 0 && !_pointerOverUI)
        {
            // Check to see if raycast hit a PC first. 
            foreach (RaycastHit hit in hits)
            {
                //Using "LayerMask.Contains()" extension method instead of writing "if ((_pCLayerMask & (1 << hit.collider.gameObject.layer)) != 0)" each time. 
                if (_pCLayerMask.Contains(hit.collider.gameObject.layer))
                {
/*                    // If PC is not currently selected PC, 
                    if (hit.transform.parent.GetInstanceID() != transform.parent.parent.parent.GetInstanceID())
                    {
                        // Activate NotSelectedSubstate. 
                        transform.parent.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(true);
                        // Deactivate SelectedSubstate (PCSelector handles activating new PC's substate). 
                        gameObject.SetActive(false);
                    }*/

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }

            // Check for enemy clicks second. 
            foreach (RaycastHit hit in hits)
            {
                if (_enemyLayerMask.Contains(hit.transform.parent.gameObject.layer))
                {
                    //RunToEnemyState runToEnemyState = states.gameObject.GetComponentInChildren<RunToEnemyState>(true);

                    // Make sure to get the right parent for the transform. 
                    // Set fighting variables. 
                    _approachEnemyState.Target = hit.transform.parent;

                    // Switch current state (if not in run to enemy state already). 
                    if (transform.parent.GetInstanceID() != _approachEnemyState.transform.GetInstanceID())
                    {
                        SwitchToState(_approachEnemyState.gameObject);
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
                    // Make sure container hasn't been looted, 
                    if (!hit.transform.GetComponent<LootContainer>().Looted &&
                        // and isn't currently being looted
                        !hit.transform.GetComponent<LootContainer>().IsBeingLooted)
                    {
                        //if (hit.transform.parent != _approachLootState.LootContainerTransform)
                      //  {
                            // Set looting variables. 
                            _approachLootState.LootContainerTransform = hit.transform.parent;

                            // Switch current state. 
                           // Debug.Log($"Current state: {transform.parent.gameObject.name}, ApproachLoot state: {_approachLootState.gameObject.name}");
                            //if (transform.parent.gameObject.name != _approachLootState.gameObject.name)
                            //{
                                SwitchToState(_approachLootState.gameObject);
                           // }
                     //   }

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
                    //RunState runState = states.gameObject.GetComponentInChildren<RunState>(true);

                    // Set movement variables here.
                    // Set new destination for PC's NavMeshAgent. 
                    _navMeshAgent.destination = hit.point;

                    // Switch current state (if not in run state already). 
                    if (transform.parent.GetInstanceID() != _runState.transform.GetInstanceID())
                    {
                        SwitchToState(_runState.gameObject);
                    }

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }
        }
    }

    private void SwitchToState(GameObject state)
    {
        // Need this so you don't deactivate current state (since current state is deactivated after new state is activated).
        if (state != transform.parent.gameObject)
        {
            Debug.Log($"[SwitchToState] from {transform.parent.gameObject.name} to {state.name}");
     
            // Activate new state's Selected substate. 
            state.GetComponentInChildren<SelectedSubstate>(true).gameObject.SetActive(true);
            // Deactivate new state's NotSelected substate. 
            state.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(false);

            // Activate new state. 
            state.SetActive(true);
            // Deactivate current state. 
            transform.parent.gameObject.SetActive(false);
        }
    }
}