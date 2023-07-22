using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField]
    private SOListSOPC _currentTeamSO;

    public SOListSOPC CurrentTeamSO { get { return _currentTeamSO; } }
    public GameState ActiveState { get; private set; }

    private void Start()
    {
        // Start game in Pause state, in main menu. 
        // FOR NOW, start in home state for testing, until main menu is built. 
        ChangeGameStateTo(/*Pause*/Home());
    }

    public void ChangeGameStateTo(GameState gameState)
    {
/*        if (ActiveState != null)
        {
            ActiveState.Exit();
        }*/

        ActiveState = gameState;

        // Initialize new game state. 
        ActiveState.SetActionMaps();
        ActiveState.SetTimeScale();

        Debug.Log($"Game state changed to {ActiveState.GetType()}");
    }

    public GamePauseState Pause() { return new GamePauseState(this); }
    public GameHomeState Home() { return new GameHomeState(this); }
    public GameHomeMenusState HomeMenus() { return new GameHomeMenusState(this); }
    public GameCombatState Combat() { return new GameCombatState(this); }
    public GameCombatMenusState CombatMenus() { return new GameCombatMenusState(this); }
    public GameBuildState Build() { return new GameBuildState(this); }




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