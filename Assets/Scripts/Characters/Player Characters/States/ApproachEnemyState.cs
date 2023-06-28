using UnityEngine;
using UnityEngine.AI;

public class ApproachEnemyState : MonoBehaviour
{
    public Transform Target { get; set; }

    [SerializeField]
    private GameObject _combatState;

    // Only SerializeField for testing. 
    [SerializeField]
    private float _weaponRange = 5f;

    private NavMeshAgent _agent;
    private Transform _pcTransform;

    private void OnEnable()
    {
        _agent = transform.parent.parent.gameObject.GetComponent<NavMeshAgent>();
        _pcTransform = _agent.transform;

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
        // TODO - What if target dies while you're running toward them? 

        // Check if within range of target (depends on what weapon you're using). 
        if (CharacterWithinRangeOfEnemy())
        {
            // Unset NavMeshAgent destination? Can't set Vector3 to null. 
            _agent.isStopped = true;
            _agent.ResetPath();

            // Set Target in CombatState. 
            _combatState.GetComponent<CombatState>().Target = Target; 

            StateSwitcher.Switch(gameObject, _combatState);
        }
    }

    private bool CharacterWithinRangeOfEnemy()
    {
        // TODO - Check if Target becomes null. 
        if (Vector3.Distance(_pcTransform.position, Target.position) < _weaponRange)
        {
            return true;
        }
        return false;
    }
}