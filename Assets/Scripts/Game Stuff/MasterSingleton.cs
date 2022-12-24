using UnityEngine;

public class MasterSingleton : MonoBehaviour
{
    public static MasterSingleton Instance { get; private set; }

    public InputManager InputManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Found more than one MasterSingleton in the scene.");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        InputManager = GetComponentInChildren<InputManager>();

        DontDestroyOnLoad(gameObject);
    }
}