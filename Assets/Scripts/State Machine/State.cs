using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    protected T _stateMachine;

    public State(T characterController)
    {
        _stateMachine = characterController;
    }

    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}