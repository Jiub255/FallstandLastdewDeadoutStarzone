using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : StateMachine<PlayerController>
{
//    public static event Action<Transform> OnSelectPC;
//    public static event Action OnDeselectPC;

    [SerializeField, Header("Idle/Movement State Variables")]
    private float _sightDistance = 5f;
    [SerializeField]
    private float _stoppingDistance = 1f;

    [SerializeField, Header("Loot State Variables")]
    private float _lootDistance = 0.1f;
    // For getting the duration of the loot animation. 
    [SerializeField]
    private AnimationClip _lootAnimation;
    [SerializeField]
    private InventorySO _inventorySO;
    [SerializeField]
    private LootTimer _lootTimer;
//    public LootContainer LootContainer { get; set; }

    [Header("Combat State Variables")]
    // Only serialized for testing. Will get this value from weapon/stats eventually. 
    [SerializeField]
    private float _weaponRange = 5f;
    // Only serialized for testing. Will get this value from weapon/stats eventually. 
    [SerializeField]
    private float _attackDuration = 1f;
//    public Transform Target { get; set; }

    [Header("Any State Variables")]
    public Animator Animator;
    public SelectedPCIcon SelectedPCIcon;
    public PathNavigator PathNavigator;
//    public NavMeshAgent NavMeshAgent;
    // Use this bool for "any state" stuff here, and check for _selected == false in idle state, since that state
    // acts differently when not selected (unselected characters automatically loot and fight things within range). 
    public bool Selected { get; private set; } = false;

    [Header("Individual Layers")]
    public LayerMask PCLayerMask;
    public LayerMask EnemyLayerMask;
    public LayerMask LootContainerLayerMask;
    public LayerMask GroundLayerMask;

    private EventSystem _eventSystem;
    // Need to use this bool and check in update to avoid a unity error. 
    private bool _pointerOverUI = false;
    private InputAction _mousePositionAction;

    // States
    public PlayerIdleState Idle() { return new PlayerIdleState(this, _sightDistance); } 
    public PlayerCombatState Combat(Transform target) { return new PlayerCombatState(this, target, _attackDuration); } 
    public PlayerLootState Loot(LootContainer lootContainer) { return new PlayerLootState(this, lootContainer, _lootAnimation, _inventorySO, _lootTimer); }
    public PlayerApproachLocationState ApproachLocation(Vector3 destination) { return new PlayerApproachLocationState(this, destination, _stoppingDistance); } 
    public PlayerApproachEnemyState ApproachEnemy(Transform target) { return new PlayerApproachEnemyState(this, target, _weaponRange); } 
    public PlayerApproachLootState ApproachLoot(LootContainer lootContainer) { return new PlayerApproachLootState(this, lootContainer, _lootDistance); } 

    // Just for testing. 
/*    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Debug.Log($"Active state: {_activeState.GetType()}");
    }*/

    public override void Update()
    {
        base.Update();

        _pointerOverUI = _eventSystem.IsPointerOverGameObject();
    }

    private void Start/*OnEnable*/()
    {
//        NavMeshAgent = transform.root.GetComponent<NavMeshAgent>();
        _mousePositionAction = S.I.IM.PC.World.MousePosition;
        _eventSystem = EventSystem.current;

        // started is single or double click, canceled is single click only. 
        S.I.IM.PC.Home.SelectOrCenter./*canceled*/performed += HandleClick;
        S.I.IM.PC.Scavenge.Select.performed += HandleClick;

        // Activate or deactivate selected pc icon. 
        SelectedPCIcon.ActivateIcon(Selected);

        // Start in idle state.
        ChangeStateTo(Idle());
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

    public void SetSelected(bool selected)
    {
        Selected = selected;
        SelectedPCIcon.ActivateIcon(selected);
    }

    private void HandleClick(InputAction.CallbackContext context)
    {
        if (Selected)
        {
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
                    if (PCLayerMask.Contains(hit.collider.gameObject.layer))
                    {
                        // TODO - How to handle selecting PCs? Want at most one selected at one time. Having each with its own selected bool
                        // could cause errors with too many selected. Maybe use a scriptable object? Then just have each PC check if it is the selected one
                        // inside of HandleClick? 
                        // Using PCSelector for now, might try some other new idea later. 

                        // Return so that multiple hits don't get called. 
                        // PCSelector handles the actual PC hits. In its own class and not here since it can happen with no PC selected. 
                        return;
                    }
                }

                // Check for enemy clicks second. 
                foreach (RaycastHit hit in hits)
                {
                    if (EnemyLayerMask.Contains(hit.collider.gameObject.layer))
                    {
                        ChangeStateTo(ApproachEnemy(hit.transform));

                        // Return so that multiple hits don't get called. 
                        return;
                    }
                }

                // Check for loot clicks third. 
                foreach (RaycastHit hit in hits)
                {
                    if (LootContainerLayerMask.Contains(hit.collider.gameObject.layer))
                    {
                        LootContainer lootContainer = hit.transform.GetComponent<LootContainer>();
                        if (lootContainer != null)
                        {
                            // Make sure container hasn't been looted and isn't currently being looted, 
                            if (!lootContainer.Looted && !lootContainer.IsBeingLooted)
                            {
                                Debug.Log($"Changing state to ApproachLoot from PlayerController. ");
                                ChangeStateTo(ApproachLoot(lootContainer));

                                // Return so that multiple hits don't get called. 
                                return;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("No LootContainer found. ");
                        }
                    }
                }

                // Check for ground clicks last. 
                foreach (RaycastHit hit in hits)
                {
                    if (GroundLayerMask.Contains(hit.collider.gameObject.layer))
                    {
                        ChangeStateTo(ApproachLocation(hit.point));

                        // Return so that multiple hits don't get called. 
                        return;
                    }
                }
            }
        }
    }
}