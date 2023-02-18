using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimation : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    
    private void Awake()
    {
        _agent = GetComponentInParent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
}