using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// TODO - have a list of SOPCStates here, and construct the new states from those as templates. 
// But how exactly? Especially for enemies, how to construct states from list of SOs? 
public class PCStateMachine : CharacterStateMachine<PCStateMachine>
{
    [SerializeField, Header("Idle/Movement State Variables")]
    // TODO - Put PC state SOs on SOPC, and only reference that here. 
    private SOPCData _pcSO;
    // For checking CurrentMenuPC instance ID on item events. 
    [SerializeField]
    private SOListSOPC _currentTeamSO;

/*    // SHARED
    private float _sightDistance = 5f;
    [SerializeField]
    // SHARED
    private float _stoppingDistance = 1f;*/

    [Header("Combat/ApproachEnemy State Variables")]
    // Only serialized for testing. Will get this value from weapon/stats eventually. 
    [SerializeField]
    // INDIVIDUAL
    private float _weaponRange = 5f;
    // Only serialized for testing. Will get this value from weapon/stats eventually. 
    [SerializeField]
    // INDIVIDUAL
    private float _attackDuration = 1f;

    [SerializeField, Header("Loot/ApproachLoot State Variables")]
    // TODO - Make the loot animation loop, and control the loot duration based on scavenging stat of PC. So it'll be individual data and not on this SO. 
    // For getting the duration of the loot animation. 
    // SHARED
    private AnimationClip _lootAnimation;
    private LootTimer _lootTimer;

    [Header("Any State Variables")]
    private Animator _animator;
    private SelectedPCIcon _selectedPCIcon;
    private PathNavigator _pathNavigator;
    // Use this bool for "any state" stuff here, and check for _selected == false in idle state, since that state
    // acts differently when not selected (unselected characters automatically loot and fight things within range). 
    // TODO - Keep this on SOPC? 
    // INDIVIDUAL
    public bool Selected { get; private set; } = false;

    [Header("Individual Layers")]
    [SerializeField]
    public LayerMask _pCLayerMask;
    [SerializeField]
    public LayerMask _enemyLayerMask;
    [SerializeField]
    public LayerMask _lootContainerLayerMask;
    [SerializeField]
    public LayerMask _exitLayerMask;
    [SerializeField]
    public LayerMask _groundLayerMask;

    // Using property to have a centralized place to reference this SO in the player game object. And for PCSelector. 
    public SOPCData PCSO { get { return _pcSO; } }
    // Using property to have a centralized place to reference this SO in the player game object. 
    public SOListSOPC CurrentTeamSO { get { return _currentTeamSO; } }
    public Animator Animator { get { return _animator; } private set { _animator = value; } }
    public SelectedPCIcon SelectedPCIcon { get { return _selectedPCIcon; } private set { _selectedPCIcon = value; } }
    public PathNavigator PathNavigator { get { return _pathNavigator; } private set { _pathNavigator = value; } }
    public LayerMask PCLayerMask { get { return _pCLayerMask; } }
    public LayerMask EnemyLayerMask { get { return _enemyLayerMask; } }
    public LayerMask LootContainerLayerMask { get { return _lootContainerLayerMask; } }
    public LayerMask ExitLayerMask { get { return _exitLayerMask; } }
    public LayerMask GroundLayerMask { get { return _groundLayerMask; } }

    // For checking if mouse is over UI. 
    private EventSystem _eventSystem;
    private bool _pointerOverUI = false;
    private InputAction _mousePositionAction;

    // GET DATA FOR THESE FROM SOs. 
    // Data from SOPCMovementState. 
    public PCApproachLocationState ApproachLocation(Vector3 destination) { return new PCApproachLocationState(this, destination); } 
    public PCIdleState Idle() { return new PCIdleState(this, _pcSO.PCMovementStateSO); }

    // Data from SOPCCombatState. 
    public PCApproachEnemyState ApproachEnemy(Transform target) { return new PCApproachEnemyState(this, target, _weaponRange); } 
    public PCCombatState Combat(Transform target) { return new PCCombatState(this, target, _attackDuration); }

    // Data from SOPCLootState. 
    public PCApproachLootState ApproachLoot(LootContainer lootContainer) { return new PCApproachLootState(this, lootContainer); } 
    public PCLootState Loot(LootContainer lootContainer) { return new PCLootState(this, lootContainer, _lootAnimation, _lootTimer); }

    private void Start/*OnEnable*/()
    {
        _mousePositionAction = S.I.IM.PC.Camera.MousePosition;
        _eventSystem = EventSystem.current;
        _lootTimer = GetComponentInChildren<LootTimer>();
        _selectedPCIcon = GetComponentInChildren<SelectedPCIcon>();
        _animator = GetComponentInChildren<Animator>();
        _pathNavigator = GetComponent<PathNavigator>();

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

    // TODO - Subscribe/unsubscribe to item events based on this? Then won't need to check CurrentMenuPC instance ID. 
    // OR, send the events to SOListSOPC or some other PC manager, and it will do whatever to the specific PC? 
    // PCSelector and PlayerIdleState set the "Selected" bool when PC selection changes. 
    public void SetSelected(bool selected)
    {
        Selected = selected;
        SelectedPCIcon.ActivateIcon(selected);
    }

    // TODO - Handle this through PCItemUseManager too? Maybe not, using the bool in PlayerIdleState anyway for all non-selected PCs. 
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