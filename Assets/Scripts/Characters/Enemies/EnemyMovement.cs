using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float attackDistance = 3f;

    private NavMeshAgent navMeshAgent;

    private Transform targetPC;

    public static event Action<Transform, Transform> onReachedPC;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        ChooseRandomTarget();
    }

    private void Update()
    {
        if (targetPC != null)
        {
            navMeshAgent.SetDestination(targetPC.transform.position);
        }
        else
        {
            ChooseRandomTarget(); 
        }

        if (Vector3.Distance(transform.position, targetPC.transform.position) <= attackDistance)
        {
            onReachedPC.Invoke(transform, targetPC);
        }
    }

    private void ChooseRandomTarget()
    {
        // Choose random PC from all available as starting target (for now)
        List<GameObject> potentialTargets = new List<GameObject>();
        potentialTargets.AddRange(GameObject.FindGameObjectsWithTag("PlayerCharacter"));
        int randomIndex = UnityEngine.Random.Range(0,potentialTargets.Count);
        targetPC = potentialTargets[randomIndex].transform;
    }
}