using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PCStateMachine
{
    private InputAction MousePositionAction { get; }
    private SelectedPCIcon SelectedPCIcon { get; }
    public SOPCData PCDataSO { get; } 
    public Animator Animator { get; }
    public PathNavigator PathNavigator{ get; }
    private InputManager InputManager { get; }

    public PCStateMachine(SOPCData pcDataSO, InputManager inputManager)
    {
        PCDataSO = pcDataSO;
        InputManager = inputManager;
        GameObject pcGO = pcDataSO.PCInstance;
        SelectedPCIcon = pcGO.GetComponentInChildren<SelectedPCIcon>();
        Animator = pcGO.GetComponentInChildren<Animator>();
        PathNavigator = pcGO.GetComponent<PathNavigator>();

        MousePositionAction = inputManager.PC.Camera.MousePosition;

        // Activate or deactivate selected pc icon. 
        SelectedPCIcon.ActivateIcon(pcDataSO.Selected);

        // Start in idle state.
        ChangeStateTo(Idle());
    }

    // GET DATA FOR THESE FROM SOs. 
    // Data from SOPCMovementState. 
    public PCApproachLocationState ApproachLocation(Vector3 destination) { return new PCApproachLocationState(this, InputManager, destination); } 
    public PCIdleState Idle() { return new PCIdleState(this, InputManager, PCDataSO.PCMovementStateSO); }

    // Data from SOPCCombatState. 
    public PCApproachEnemyState ApproachEnemy(Transform target) { return new PCApproachEnemyState(this, InputManager, target); } 
    public PCCombatState Combat(Transform target) { return new PCCombatState(this, InputManager, target); }

    // Data from SOPCLootState. 
    public PCApproachLootState ApproachLoot(LootContainer lootContainer) { return new PCApproachLootState(this, InputManager, lootContainer); } 
    public PCLootState Loot(LootContainer lootContainer) { return new PCLootState(this, InputManager, lootContainer); }

    public void SetSelected(bool selected)
    {
        PCDataSO.Selected = selected;
        SelectedPCIcon.ActivateIcon(selected);
    }

    public void ChangeStateTo(PCState state)
    {
        if (PCDataSO.ActiveState != null)
        {
            PCDataSO.ActiveState.Exit();
        }

        PCDataSO.ActiveState = state;

//        Debug.Log($"Changed state to: {PCDataSO.ActiveState.GetType()}");
    }

    // 
    /// <summary>
    /// Checks if player clicked on an enemy, loot, scene exit, or ground, then changes state accordingly. PC clicks are handled by PCSelector (before this ever gets run). <br/>
    /// Called by PCManager's HandleClick. 
    /// </summary>
    public void HandleClick()
    {
        // RaycastAll to see what was hit. 
        RaycastHit[] hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(MousePositionAction.ReadValue<Vector2>()),
            1000);

        // If raycast hits anything, 
        if (hits.Length > 0)
        {
            // Check to see if raycast hit a PC first. 
            foreach (RaycastHit hit in hits)
            {
                //Using "LayerMask.Contains()" extension method instead of writing "if ((_pCLayerMask & (1 << hit.collider.gameObject.layer)) != 0)" each time. 
                if (PCDataSO.PCSharedDataSO.PCLayerMask.Contains(hit.collider.gameObject.layer))
                {
                    // Return so that multiple hits don't get called. 
                    // PCSelector handles the actual PC hits. In its own class and not here since it can happen with no PC selected. 
                    return;
                }
            }

            // Check for enemy clicks second. 
            foreach (RaycastHit hit in hits.Where(hit => PCDataSO.PCSharedDataSO.EnemyLayerMask.Contains(hit.collider.gameObject.layer)))
            {
                ChangeStateTo(ApproachEnemy(hit.transform));
                // Return so that multiple hits don't get called. 
                return;
            }

            // Check for loot clicks third. 
            foreach (RaycastHit hit in hits.Where(hit => PCDataSO.PCSharedDataSO.LootContainerLayerMask.Contains(hit.collider.gameObject.layer)))
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

            // Check for exit clicks fourth. 
            // TODO - Would it be more performant to just skip the scene check? 
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene() == UnityEngine.SceneManagement.SceneManager.GetSceneByName("ScavengingScene"))
            {
                foreach (RaycastHit hit in hits.Where(hit => PCDataSO.PCSharedDataSO.ExitLayerMask.Contains(hit.collider.gameObject.layer)))
                {
                    ChangeStateTo(ApproachLocation(hit.point));
                    // Return so that multiple hits don't get called. 
                    return;
                }
            }

            // Check for ground clicks last. 
            foreach (var hit in hits.Where(hit => PCDataSO.PCSharedDataSO.GroundLayerMask.Contains(hit.collider.gameObject.layer)))
            {
                ChangeStateTo(ApproachLocation(hit.point));
                // Return so that multiple hits don't get called. 
                return;
            }
        }
    }
}