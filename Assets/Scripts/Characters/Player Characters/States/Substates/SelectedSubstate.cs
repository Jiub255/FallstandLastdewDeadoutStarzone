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

        // Deactivate selected icon. 
        transform.parent.parent.parent.GetComponentInChildren<SelectedPCIcon>().DeactivateIcon();

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
       // Debug.Log("Clicked");

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
                    if (hit.collider.gameObject.GetInstanceID() != transform.parent.parent.parent.gameObject.GetInstanceID())
                    {
                        // Activate NotSelectedSubstate. 
                        transform.parent.GetChild(1).gameObject.SetActive(true);

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
                if (_enemyLayerMask.Contains(hit.collider.gameObject.layer))
                {
                    RunToEnemyState runToEnemyState = states.gameObject.GetComponentInChildren<RunToEnemyState>(true);

                    // Make sure to get the right parent for the transform. 
                    // Set fighting variables. 
                    runToEnemyState.Target = hit.transform/*.parent*/;

                    // Switch current state (if not in run to enemy state already). 
                    if (transform.parent.name != "Run To Enemy")
                    {
                        // Deactivate current state. 
                        transform.parent.gameObject.SetActive(false);

                        // Activate RunToEnemyState. 
                        runToEnemyState.gameObject.SetActive(true);
                        // Activate its selected substate. 
                        runToEnemyState.transform.GetChild(0).gameObject.SetActive(true);
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
                    RunToLootState runToLootState = states.gameObject.GetComponentInChildren<RunToLootState>(true);
                    
                    // Set looting variables. 
                    runToLootState.LootContainerTransform = hit.transform.parent;

                    // Switch current state (if not in run to loot state already). 
                    if (transform.parent.name != "Run To Loot")
                    {
                        // Deactivate current state. 
                        transform.parent.gameObject.SetActive(false);

                        // Activate RunToLootState. 
                        runToLootState.gameObject.SetActive(true);
                        // Activate its selected substate. 
                        runToLootState.transform.GetChild(0).gameObject.SetActive(true);
                    }

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }

            // Check for ground clicks last. 
            foreach (RaycastHit hit in hits)
            {
                if (_groundLayerMask.Contains(hit.collider.gameObject.layer))
                {
                    // Set movement variables here.
                    // Set new destination for PC's NavMeshAgent. 
                    transform.parent.parent.parent.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = hit.point;

                    // Switch current state (if not in run state already). 
                    if (transform.parent.name != "Run")
                    {
                        // Deactivate current state. 
                        transform.parent.gameObject.SetActive(false);

                        // Activate Run state and selected substate. 
                        RunState runState = states.gameObject.GetComponentInChildren<RunState>(true);
                        runState.gameObject.SetActive(true);
                        runState.transform.GetChild(0).gameObject.SetActive(true);
                    }

                    // Return so that multiple hits don't get called. 
                    return;
                }
            }
        }
    }
}