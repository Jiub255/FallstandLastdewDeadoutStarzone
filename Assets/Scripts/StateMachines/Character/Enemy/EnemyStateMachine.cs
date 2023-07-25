using UnityEngine;

public abstract class EnemyStateMachine : MonoBehaviour
{
    [SerializeField]
    private SOEnemyData _enemyDataSO;

    private PathNavigator _pathNavigator;
    private EnemyState _activeState;

    public PathNavigator PathNavigator { get { return _pathNavigator; } }

    public EnemyCombatState Combat(Transform target) { return new EnemyCombatState(this, target, _enemyDataSO.EnemyCombatStateSO); }
    public EnemyApproachPCState ApproachPC() { return new EnemyApproachPCState(this, _enemyDataSO.EnemyCombatStateSO); }

    // Needs to be in Start and not Awake so the PCs have time to instantiate, then the enemy can choose a target PC
    // in its EnemyApproachPCState constructor. 
    private void /*Awake*/Start()
    {
        _pathNavigator = GetComponent<PathNavigator>();

        ChangeStateTo(ApproachPC());
    }

    public virtual void Update()
    {
        if (_activeState != null)
        {
            _activeState.Update();
        }
        else
        {
            Debug.LogWarning($"No active state");
        }
    }

    public virtual void FixedUpdate()
    {
        if (_activeState != null)
        {
            _activeState.FixedUpdate();
        }
        else
        {
            Debug.LogWarning($"No active state");
        }
    }

    public void ChangeStateTo(EnemyState state)
    {
        if (_activeState != null)
        {
            _activeState.Exit();
        }

        _activeState = state; 

//        Debug.Log($"{gameObject.name} changed state to: {_activeState.GetType()}");
    }
}