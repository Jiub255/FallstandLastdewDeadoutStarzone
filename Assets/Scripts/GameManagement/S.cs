using UnityEngine;

// S is for Singleton
public class S : MonoBehaviour
{
    // I is for Instance
    public static S I { get; private set; }

    public InputManager IM { get; private set; }
    public GameManager GameManager { get; private set; }
    public SceneTransitionManager SceneTransitionManager { get; private set; }

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
        GameManager = GetComponentInChildren<GameManager>();
        SceneTransitionManager = GetComponentInChildren<SceneTransitionManager>();

        DontDestroyOnLoad(gameObject);
    }
}