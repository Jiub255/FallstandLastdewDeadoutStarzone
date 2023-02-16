using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveToPCState : MonoBehaviour
{
    private Transform _target;

    [SerializeField]
    private float _attackRadius = 2f;

    [SerializeField]
    private GameObject _enemyCombatState;

    private NavMeshAgent _agent;
    private Transform _transform;

    public float AttackRadius { get { return _attackRadius; } }

    private void OnEnable()
    {
        _agent = transform.parent.GetComponentInParent<NavMeshAgent>();
        _transform = _agent.transform;
    }

    private void Start()
    {
        // Set random (?) PC as target. 
        _target = ChooseRandomTarget();
        // Set target as NavMeshAgent destination
        _agent.SetDestination(_target.position);
    }

    private void Update()
    {
        if (Vector3.Distance(_target.position, _transform.position) <= _attackRadius)
        {
            // Set the Target in EnemyCombatState. 
            _enemyCombatState.GetComponent<EnemyCombatState>().Target = _target; 

            // Switch to EnemyCombatState. 
            _enemyCombatState.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        // Update destination in FixedUpdate so it's less costly. Probably unnecessary. 
        _agent.SetDestination(_target.position);
        _transform.LookAt(_target);
    }

    private Transform ChooseRandomTarget()
    {
        // Choose random PC from all available as starting target (for now)
        List<GameObject> potentialTargets = new List<GameObject>();
        potentialTargets.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));
        int randomIndex = Random.Range(0, potentialTargets.Count);
        return potentialTargets[randomIndex].transform;
    }
}