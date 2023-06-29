using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private PathNavigator _pathNavigator;
//    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    
    private void Awake()
    {
        _pathNavigator = GetComponentInParent<PathNavigator>();
//        _navMeshAgent = GetComponentInParent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float speed = _pathNavigator.Moving ? 1 : 0;
        _animator.SetFloat("Speed", speed);
    }
}