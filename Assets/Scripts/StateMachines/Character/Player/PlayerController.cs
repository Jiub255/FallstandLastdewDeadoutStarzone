using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// TODO - have a list of SOPCStates here, and construct the new states from those as templates. 
// But how exactly? Especially for enemies, how to construct states from list of SOs? 
public class PlayerController : CharacterStateMachine<PlayerController>
{
    [SerializeField, Header("Idle/Movement State Variables")]
    // SHARED
    private float _sightDistance = 5f;
    [SerializeField]
    // SHARED
    private float _stoppingDistance = 1f;

    [SerializeField, Header("Loot State Variables")]
    // SHARED
    private float _lootDistance = 0.1f;
    // For getting the duration of the loot animation. 
    [SerializeField]
    // SHARED
    private AnimationClip _lootAnimation;
    [SerializeField]
    // SHARED
    private LootTimer _lootTimer;

    [Header("Combat State Variables")]
    // Only serialized for testing. Will get this value from weapon/stats eventually. 
    [SerializeField]
    // INDIVIDUAL
    private float _weaponRange = 5f;
    // Only serialized for testing. Will get this value from weapon/stats eventually. 
    [SerializeField]
    // INDIVIDUAL
    private float _attackDuration = 1f;

    [Header("Any State Variables")]
    // SHARED
    public Animator Animator;
    // SHARED
    public SelectedPCIcon SelectedPCIcon;
    // SHARED
    public PathNavigator PathNavigator;
    // Use this bool for "any state" stuff here, and check for _selected == false in idle state, since that state
    // acts differently when not selected (unselected characters automatically loot and fight things within range). 
    // SHARED
    public bool Selected { get; private set; } = false;

    [Header("Individual Layers")]
    // SHARED
    public LayerMask PCLayerMask;
    // SHARED
    public LayerMask EnemyLayerMask;
    // SHARED
    public LayerMask LootContainerLayerMask;
    // SHARED
    public LayerMask ExitLayerMask;
    // SHARED
    public LayerMask GroundLayerMask;

    // For checking if mouse is over UI. 
    private EventSystem _eventSystem;
    private bool _pointerOverUI = false;
    private InputAction _mousePositionAction;

    // TODO - have a list of SOPCStates here, and construct the new states from those as templates. 
    // Have each state have a stateArguments class to pass to constructor? Or just pass the SO? Just pass the SO. 
    // The SO is not the state, just the state's data. 
    // States
    // GET DATA FOR THESE FROM SOs. 
    public PlayerIdleState Idle() { return new PlayerIdleState(this, _sightDistance); } 
    public PCState Combat(Transform target) { return new PCState(this, target, _attackDuration); } 
    public PlayerLootState Loot(LootContainer lootContainer) { return new PlayerLootState(this, lootContainer, _lootAnimation, _lootTimer); }
    public PlayerApproachLocationState ApproachLocation(Vector3 destination) { return new PlayerApproachLocationState(this, destination, _stoppingDistance); } 
    public PlayerApproachEnemyState ApproachEnemy(Transform target) { return new PlayerApproachEnemyState(this, target, _weaponRange); } 
    public PlayerApproachLootState ApproachLoot(LootContainer lootContainer) { return new PlayerApproachLootState(this, lootContainer, _lootDistance); } 

    private void Start/*OnEnable*/()
    {
        _mousePositionAction = S.I.IM.PC.Camera.MousePosition;
        _eventSystem = EventSystem.current;

        // started is single or double click, canceled is single click only. 
        S.I.IM.PC.World.SelectOrCenter.canceled/*performed*/ += HandleClick;

        // Activate or deactivate selected pc icon. 
        SelectedPCIcon.ActivateIcon(Selected);

        // Start in idle state.
        ChangeStateTo(Idle());
    }

    private void OnDisable()
    {
        S.I.IM.PC.World.SelectOrCenter.canceled/*performed*/ -= HandleClick;
    }

    public override void Update()
    {
        base.Update();

        _pointerOverUI = _eventSystem.IsPointerOverGameObject();
    }

    // Just for testing. 
/*    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Debug.Log($"Active state: {_activeState.GetType()}");
    }*/

    // PCSelector and PlayerIdleState set the "Selected" bool when PC selection changes. 
    public void SetSelected(bool selected)
    {
        Selected = selected;
        SelectedPCIcon.ActivateIcon(selected);
    }

    // Checks if player clicked on an enemy, loot, scene exit, or ground. PC clicks are handled by PCSelector. 
    private void HandleClick(InputAction.CallbackContext context)
    {
        // Only let the selected PC do these checks. 
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

                // Check for exit clicks fourth. 
                // TODO - Would it be more performant to just skip the scene check? 
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene() == UnityEngine.SceneManagement.SceneManager.GetSceneByName("ScavengingScene"))
                {
                    foreach (RaycastHit hit in hits)
                    {
                        if (ExitLayerMask.Contains(hit.collider.gameObject.layer))
                        {
                            ChangeStateTo(ApproachLocation(hit.point));

                            // Return so that multiple hits don't get called. 
                            return;
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