using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateMachine : MonoBehaviour
{
    // Make a game state machine. Similar to the character state machine, but not sure if update methods are needed. 
    // Might just have an enter and maybe exit method. 

    public GameState ActiveState { get; private set; }

    [SerializeField]
    private SOListSOPC _currentTeamSO;
    public SOListSOPC CurrentTeamSO { get { return _currentTeamSO; } }

    public void ChangeStateTo(GameState gameState)
    {
/*        if (ActiveState != null)
        {
            ActiveState.Exit();
        }*/

        ActiveState = gameState;

        // Initialize new game state. 
        ActiveState.SetActionMaps();
        ActiveState.SetTimeScale();
        ActiveState.ResetPCInstanceReferencesOnSOs();

        Debug.Log($"Game state changed to {ActiveState.GetType()}");
    }

    public GamePauseState Pause() { return new GamePauseState(this); }






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