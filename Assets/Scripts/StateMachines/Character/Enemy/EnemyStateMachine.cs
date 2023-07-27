using UnityEngine;

public abstract class EnemyStateMachine : MonoBehaviour
{
    [SerializeField]
    private SOEnemyData _enemyDataSO;

    private SOEnemyData EnemyDataSO { get { return _enemyDataSO; } }
    private EnemyState ActiveState { get; set; }
    public PathNavigator PathNavigator { get; set; }

    public EnemyCombatState Combat(Transform target) { return new EnemyCombatState(this, target, EnemyDataSO.EnemyCombatStateSO); }
    public EnemyApproachPCState ApproachPC() { return new EnemyApproachPCState(this, EnemyDataSO.EnemyCombatStateSO); }

    // Needs to be in Start and not Awake so the PCs have time to instantiate, then the enemy can choose a target PC
    // in its EnemyApproachPCState constructor. 
    private void /*Awake*/Start()
    {
        PathNavigator = GetComponent<PathNavigator>();

        ChangeStateTo(ApproachPC());
    }

    public virtual void Update()
    {
        if (ActiveState != null)
        {
            ActiveState.Update();
        }
        else
        {
            Debug.LogWarning($"No active state");
        }
    }

    public virtual void FixedUpdate()
    {
        if (ActiveState != null)
        {
            ActiveState.FixedUpdate();
        }
        else
        {
            Debug.LogWarning($"No active state");
        }
    }

    public void ChangeStateTo(EnemyState state)
    {
        if (ActiveState != null)
        {
            ActiveState.Exit();
        }

        ActiveState = state; 

//        Debug.Log($"{gameObject.name} changed state to: {_activeState.GetType()}");
    }
}