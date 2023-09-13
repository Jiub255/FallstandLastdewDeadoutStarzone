using System;
using UnityEngine;

public class UIStartMenu : MonoBehaviour
{
	public static event Action OnContinue;
	public static event Action OnLoad;
	public static event Action OnNew;
	public static event Action OnExit;

	public void OnContinuePressed()
    {
		OnContinue?.Invoke();
    }

	public void OnLoadPressed()
    {
		OnLoad?.Invoke();
    }

	public void OnNewPressed()
    {
		OnNew?.Invoke();
    }

	public void OnExitPressed()
    {
		OnExit?.Invoke();
    }
}