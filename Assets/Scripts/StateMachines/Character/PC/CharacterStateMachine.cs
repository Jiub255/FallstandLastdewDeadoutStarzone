public abstract class CharacterStateMachine<T>/* : MonoBehaviour where T : MonoBehaviour*/
{
    protected CharacterState<T> _activeState;

/*    public virtual void Update()
    {
        if (_activeState != null)
        {
            _activeState.Update();
        }
        else
        {
            Debug.LogWarning($"No active state");
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
            Debug.LogWarning($"No active state");
        }
    }*/

    public void ChangeStateTo(CharacterState<T> state)
    {
        if (_activeState != null)
        {
            _activeState.Exit();
        }

        _activeState = state; 

//        Debug.Log($"{gameObject.name} changed state to: {_activeState.GetType()}");
    }
}