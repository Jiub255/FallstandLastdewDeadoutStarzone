public abstract class CharacterState<T>/* where T : MonoBehaviour*/
{
    protected T _stateMachine;

    public CharacterState(T characterController)
    {
        _stateMachine = characterController;
    }

    /// <summary>
    /// Selected bool only used for PCs, not enemies. Hence the default parameter. 
    /// </summary>
    /// <param name="selected"></param>
    public abstract void Update(bool selected = false);
    public abstract void FixedUpdate(bool selected = false);
    public abstract void Exit();
}