using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : MonoBehaviour
{
    protected State<T> _activeState { get; private set; }

    public virtual void Update()
    {
        if (_activeState != null)
        {
            _activeState.Update();
        }
        else
        {
            Debug.LogWarning($"No active state in {name}");
        }
    }

    public virtual void FixedUpdate()
    {
        if (_activeState != null)
        {
            _activeState.FixedUpdate();
        }
        else
        {
            Debug.LogWarning($"No active state in {name}");
        }
    }

    public void ChangeStateTo(State<T> state)
    {
        if (_activeState != null)
        {
            _activeState.Exit();
        }

        _activeState = state;

        Debug.Log($"{gameObject.name} changed state to: {_activeState.GetType()}");
    }
}