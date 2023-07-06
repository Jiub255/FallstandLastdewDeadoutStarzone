using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        S.I.IM.PC.Home.Quit.performed += QuitGame;
        S.I.IM.PC.Scavenge.Quit.performed += QuitGame;
    }

    private void OnDisable()
    {
        S.I.IM.PC.Home.Quit.performed -= QuitGame;
        S.I.IM.PC.Scavenge.Quit.performed -= QuitGame;
    }

    private void QuitGame(InputAction.CallbackContext obj)
    {
        Quitter.Quit();
    }
}