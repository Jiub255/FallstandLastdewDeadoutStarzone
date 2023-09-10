using UnityEngine;

public class GameStateMachine
{
    public GameState ActiveState { get; private set; }
    private InputManager InputManager { get; }
    private PCManager PCManager { get; }
    private BuildingManager BuildingManager { get; }

    public GameStateMachine(InputManager inputManager, PCManager pcManager, BuildingManager buildingManager)
    {
        InputManager = inputManager;
        PCManager = pcManager;
        BuildingManager = buildingManager;
        // Start game in Pause state, in main menu. 
        // FOR NOW, start in home state for testing, until main menu is built. 
        ChangeGameStateTo(/*Pause*/Home());
    }

    /// <summary>
    /// Changes action maps and sets time scale. 
    /// </summary>
    public void ChangeGameStateTo(GameState gameState)
    {
        if (ActiveState != null)
        {
            ActiveState.Exit();
        }

        ActiveState = gameState;

        // Initialize new game state. 
        ActiveState.SetActionMaps();
        ActiveState.SetTimeScale();

//        Debug.Log($"Game state changed to {ActiveState.GetType()}");
    }

    public GameHomeState Home() { return new GameHomeState(InputManager, PCManager); }
    public GameHomeMenusState HomeMenus() { return new GameHomeMenusState(InputManager); }
    public GameCombatState Combat() { return new GameCombatState(InputManager, PCManager); }
    public GameCombatMenusState CombatMenus() { return new GameCombatMenusState(InputManager); }
    public GameBuildState Build() { return new GameBuildState(InputManager, BuildingManager); }

    public void UpdateActiveState()
    {
        ActiveState.Update();
    }

    public void FixedUpdateActiveState()
    {
        ActiveState.FixedUpdate();
    }


    /*    public bool Paused { get; private set; } = false;

        public void Pause(bool pause)
        {
            if (Paused != pause)
            {
                Paused = pause;
                Time.timeScale = pause ? 0f : 1f;
                if (pause)
                {
                    S.I.IM.DisableAllActions();
                }
                else
                {
                    S.I.IM.ActivateStateActionMaps(S.I.IM.GameState);
                }
            }
        }

        private void Start()
        {
            S.I.IM.PC.Quit.Quit.performed += QuitGame;
        }

        private void OnDisable()
        {
            S.I.IM.PC.Quit.Quit.performed -= QuitGame;
        }

        private void QuitGame(InputAction.CallbackContext obj)
        {
            Quitter.Quit();
        }*/
}