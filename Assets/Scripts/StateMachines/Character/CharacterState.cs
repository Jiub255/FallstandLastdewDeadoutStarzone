using UnityEngine;

public abstract class CharacterState<T> where T : MonoBehaviour
{
    protected T _stateMachine;

    public CharacterState(T characterController)
    {
        _stateMachine = characterController;
    }

    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}