public class GameBuildState : GameState
{
    protected BuildingManager BuildingManager { get; }

    public GameBuildState(InputManager inputManager, BuildingManager buildingManager) : base(inputManager, 0f) 
    {
        BuildingManager = buildingManager;
    }

    public override void Update()
    {
        BuildingManager.Update();
    }

    public override void FixedUpdate() {}

    public override void Exit()
    {
        // Clear/destroy currently selected building in build mode. 
        BuildingManager.DeselectBuilding();
    }
}