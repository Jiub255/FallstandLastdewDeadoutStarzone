using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event (AudioClip)",
    menuName = "Scriptable Object/Game Events/Game Event (AudioClip)")]
public class GEAudioClip : ScriptableObject
{
    private List<GELAudioClip> listeners = new List<GELAudioClip>();

    public void Invoke(AudioClip AudioClip)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventInvoked(AudioClip);
        }
    }

    public void RegisterListener(GELAudioClip listener)
    {
        listeners.Add(listener);
    }
    public void UnregisterListener(GELAudioClip listener)
    {
        listeners.Remove(listener);
    }
}