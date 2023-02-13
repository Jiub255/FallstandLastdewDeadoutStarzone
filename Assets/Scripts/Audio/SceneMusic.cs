using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip sceneMusic;
    public GEAudioClip gameEventAudioClip;

    public void Start()
    {
        gameEventAudioClip.Invoke(sceneMusic);
    }
}