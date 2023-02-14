using UnityEngine;
using UnityEngine.AI;

public class RunToEnemyState : MonoBehaviour
{
    public Transform Target { get; set; }

    [SerializeField]
    private GameObject _combatState;

    // Only SerializeField for testing. 
    [SerializeField]
    private float _weaponRange = 3f;

    private NavMeshAgent _agent;
    private Transform _transform;

    private void OnEnable()
    {
        _agent = transform.parent.parent.gameObject.GetComponent<NavMeshAgent>();
        _transform = _agent.transform;

        // Set NavMeshAgent destination here. 
        _agent.destination = Target.position;

        // Get weapon range from current weapon, and maybe stats affect it too. 
       // _weaponRange = _playerSomething.CurrentWeapon._range; 
    }

    private void OnDisable()
    {
        // Send _target to CombatState here? 

        // Set destination to null or whatever. 

    }

    private void Update()
    {
        // What if target dies while you're running toward them? 

        // Check if within range of target (depends on what weapon you're using). 
        if (WithinRangeOfEnemy())
        {
            // Unset NavMeshAgent destination? Can't set Vector3 to null. 
            _agent.isStopped = true;
            _agent.ResetPath();

            // Set Target in CombatState. 
            _combatState.GetComponent<CombatState>().Target = Target; 

            // Activate LootState. 
            _combatState.SetActive(true);

            // Activate selected substate if currently selected. 
            if (_transform.GetChild(0).gameObject.activeSelf)
            {
                _combatState.transform.GetChild(0).gameObject.SetActive(true);
            }

            // Deactivate this state. 
            gameObject.SetActive(false);
        }
    }

    private bool WithinRangeOfEnemy()
    {
        if (Vector3.Distance(_transform.position, Target.position) < _weaponRange)
        {
            return true;
        }
        return false;
    }
}