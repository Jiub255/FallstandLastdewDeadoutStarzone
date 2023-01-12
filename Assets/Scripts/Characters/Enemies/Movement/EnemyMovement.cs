using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public static event Action<Transform, Transform> OnReachedPC;

    [SerializeField]
    private float _attackDistance = 3f;

    private NavMeshAgent _navMeshAgent;

    private Transform _targetPC;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        ChooseRandomTarget();
    }

    private void Update()
    {
        if (_targetPC != null)
        {
            _navMeshAgent.SetDestination(_targetPC.transform.position);
        }
        else
        {
            ChooseRandomTarget(); 
        }

        if (Vector3.Distance(transform.position, _targetPC.transform.position) <= _attackDistance)
        {
            OnReachedPC.Invoke(transform, _targetPC);
        }
    }

    private void ChooseRandomTarget()
    {
        // Choose random PC from all available as starting target (for now)
        List<GameObject> potentialTargets = new List<GameObject>();
        potentialTargets.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));
        int randomIndex = UnityEngine.Random.Range(0,potentialTargets.Count);
        _targetPC = potentialTargets[randomIndex].transform;
    }
}