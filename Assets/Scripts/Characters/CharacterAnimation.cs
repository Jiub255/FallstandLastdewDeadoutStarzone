using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private PathNavigator _pathNavigator;
    private Animator _animator;
    
    private void Awake()
    {
        _pathNavigator = GetComponentInParent<PathNavigator>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool("Running", _pathNavigator.Moving);
//        float speed = _pathNavigator.Moving ? 1 : 0;
//        _animator.SetFloat("Speed", speed);
    }
}