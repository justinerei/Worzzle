using UnityEngine;

public class MusicButton : MonoBehaviour
{
    public AudioClip musicClip;

    public void PlayThisPlaylist()
    {
        AudioManager manager = FindObjectOfType<AudioManager>();
        if (manager != null)
        {
            manager.PlaySFX(manager.click);
            manager.PlayMusic(musicClip);
        }
        
    }
}
