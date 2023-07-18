using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public bool Paused { get; private set; } = false;

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
    }
}