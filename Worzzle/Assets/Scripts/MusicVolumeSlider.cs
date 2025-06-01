using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }
    }

    public void UpdateVolume(float value)
    {
        AudioManager manager = FindObjectOfType<AudioManager>();
        if (manager != null)
        {
            manager.SetMusicVolume(value);
        }
    }
}
