using UnityEngine;
using UnityEngine.Events;

public class GELAudioClip : MonoBehaviour
{
    public GEAudioClip Event;
    public UnityEvent<AudioClip> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventInvoked(AudioClip AudioClip)
    {
        Response.Invoke(AudioClip);
    }
}