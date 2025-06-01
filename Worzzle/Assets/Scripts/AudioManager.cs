using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("SFX Clip")]
    public AudioClip softType;
    public AudioClip backspace;
    public AudioClip enter;
    public AudioClip invalidWord;
    public AudioClip loseState;
    public AudioClip winState;
    public AudioClip click;

    [Header("BGMusic Clip")]
    public AudioClip defaultPlaylist;
    public AudioClip relapsePlaylist;
    public AudioClip frankPlaylist;
    public AudioClip ninetyPlaylist;

    private void Start()
    {
        musicSource.clip = defaultPlaylist;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        Debug.Log("Trying to play: " + clip);
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
}


