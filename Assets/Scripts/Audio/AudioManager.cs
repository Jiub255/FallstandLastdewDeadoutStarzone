using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource sfxSource;

    private void Start()
    {
        musicSource.ignoreListenerPause = true;
    }

    public void PlaySoundEffect(AudioClip effectClip)
    {
        if (sfxSource != null)
        {
            // Using PlayOneShot so you can play multiple clips at once without cutting off the previous played one. 
            sfxSource.PlayOneShot(effectClip);
        }
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }
}