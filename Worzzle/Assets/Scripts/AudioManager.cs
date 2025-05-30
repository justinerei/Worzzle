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

    [Header("BGMusic Clip")]
    public AudioClip defaultPlaylist;
    public AudioClip relapsePlaylist;
    public AudioClip tkPlaylist;

    private void Start()
    {
        musicSource.clip = defaultPlaylist;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}


