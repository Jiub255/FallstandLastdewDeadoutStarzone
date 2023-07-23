using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// TODO - have a list of SOPCStates here, and construct the new states from those as templates. 
// But how exactly? Especially for enemies, how to construct states from list of SOs? 
public class PCController : CharacterStateMachine<PCController>
{
    [SerializeField]
    private SOPCData _pcSO;
    [SerializeField]
    private SOListSOPC _currentTeamSO;

    private Animator _animator;
    private SelectedPCIcon _selectedPCIcon;
    private PathNavigator _pathNavigator;


    // TODO - Put the three managers here. 



    // Use this bool for "any state" stuff here, and check for _selected == false in idle state, since that state
    // acts differently when not selected (unselected characters automatically loot and fight things within range). 
    public bool Selected { get; private set; } = false;
    // Using property to have a centralized place to reference this SO in the player game object. And for PCSelector. 
    // Also for states to get individual PC data (ex. scavenging skill to calculate loot duration). 
    public SOPCData PCSO { get { return _pcSO; } }
    // TODO - Is this necessary? 
    // Using property to have a centralized place to reference this SO in the player game object. 
    public SOListSOPC CurrentTeamSO { get { return _currentTeamSO; } }
    public Animator Animator { get { return _animator; } private set { _animator = value; } }
    public SelectedPCIcon SelectedPCIcon { get { return _selectedPCIcon; } private set { _selectedPCIcon = value; } }
    public PathNavigator PathNavigator { get { return _pathNavigator; } private set { _pathNavigator = value; } }

    // For checking if mouse is over UI. 
    private EventSystem _eventSystem;
    private bool _pointerOverUI = false;
    private InputAction _mousePositionAction;

    // GET DATA FOR THESE FROM SOs. 
    // Data from SOPCMovementState. 
    public PCApproachLocationState ApproachLocation(Vector3 destination) { return new PCApproachLocationState(this, destination); } 
    public PCIdleState Idle() { return new PCIdleState(this, _pcSO.PCMovementStateSO); }

    // Data from SOPCCombatState. 
    public PCApproachEnemyState ApproachEnemy(Transform target) { return new PCApproachEnemyState(this, target); } 
    public PCCombatState Combat(Transform target) { return new PCCombatState(this, target); }

    // Data from SOPCLootState. 
    public PCApproachLootState ApproachLoot(LootContainer lootContainer) { return new PCApproachLootState(this, lootContainer); } 
    public PCLootState Loot(LootContainer lootContainer) { return new PCLootState(this, lootContainer); }

    private void Start/*OnEnable*/()
    {
        _mousePositionAction = S.I.IM.PC.Camera.MousePosition;
        _eventSystem = EventSystem.current;
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
                    if (PCSO.PCSharedDataSO.PCLayerMask.Contains(hit.collider.gameObject.layer))
                    {
                        // Return so that multiple hits don't get called. 
                        // PCSelector handles the actual PC hits. In its own class and not here since it can happen with no PC selected. 
                        return;
                    }
                }

                // Check for enemy clicks second. 
                foreach (RaycastHit hit in hits)
                {
                    if (PCSO.PCSharedDataSO.EnemyLayerMask.Contains(hit.collider.gameObject.layer))
                    {
                        ChangeStateTo(ApproachEnemy(hit.transform));

                        // Return so that multiple hits don't get called. 
                        return;
                    }
                }

                // Check for loot clicks third. 
                foreach (RaycastHit hit in hits)
                {
                    if (PCSO.PCSharedDataSO.LootContainerLayerMask.Contains(hit.collider.gameObject.layer))
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
                        if (PCSO.PCSharedDataSO.ExitLayerMask.Contains(hit.collider.gameObject.layer))
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
                    if (PCSO.PCSharedDataSO.GroundLayerMask.Contains(hit.collider.gameObject.layer))
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