public class GameHomeState : GameState
{
    protected PCManager PCManager { get; }

    public GameHomeState(InputManager inputManager, PCManager pcManager) : base(inputManager, 1f)
    {
        PCManager = pcManager;
    }

    public override void Update()
    {
        PCManager.UpdateStates();
    }

    public override void FixedUpdate()
    {
        PCManager.FixedUpdateStates();
    }

    public override void Exit() {}
}