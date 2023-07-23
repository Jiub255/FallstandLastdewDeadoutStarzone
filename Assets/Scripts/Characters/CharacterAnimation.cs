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

    private void FixedUpdate()
    {
        _animator.SetBool("Running", _pathNavigator.Moving);
    }
}