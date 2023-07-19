using UnityEngine;

// S is for Singleton
public class S : MonoBehaviour
{
    // I is for Instance
    public static S I { get; private set; }

    public InputManager IM { get; private set; }
    public GameStateMachine GSM { get; private set; }
    public SceneTransitionManager STM { get; private set; }

    private void Awake()
    {
        if (I != null && I != this)
        {
            Debug.LogWarning("Found more than one S (Singleton) in the scene.");
            Destroy(this.gameObject);
            return;
        }

        I = this;

        IM = GetComponentInChildren<InputManager>();
        GSM = GetComponentInChildren<GameStateMachine>();
        STM = GetComponentInChildren<SceneTransitionManager>();

        DontDestroyOnLoad(gameObject);
    }
}