using UnityEngine;

// S is for Singleton
public class S : MonoBehaviour
{
    // I is for Instance
    public static S I { get; private set; }

    // IM if for InputManager
    public InputManager IM { get; private set; }
    public JUNKGameStateMachine GameStateMachine { get; private set; }

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
        GameStateMachine = GetComponentInChildren<JUNKGameStateMachine>();

        DontDestroyOnLoad(gameObject);
    }
}