using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class JUNKCharacterAnimation : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
}