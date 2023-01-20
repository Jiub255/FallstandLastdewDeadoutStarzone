using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        //if (S.I.IM.PC.Home.enabled)
        {
            S.I.IM.PC.Home.Quit.performed += QuitGame;
        }
        //if (S.I.IM.PC.Scavenge.enabled)
        {
            S.I.IM.PC.Scavenge.Quit.performed += QuitGame;
        }
    }

    private void OnDisable()
    {
        //if (S.I.IM.PC.Home.enabled)
        {
            S.I.IM.PC.Home.Quit.performed -= QuitGame;
        }
       // if (S.I.IM.PC.Scavenge.enabled)
        {
            S.I.IM.PC.Scavenge.Quit.performed -= QuitGame;
        }
    }

    private void QuitGame(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}