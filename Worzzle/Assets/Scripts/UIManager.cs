using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject helpPanel;  
    public GameObject musicPanel;
    
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void ShowHelpPanel()
    {
        audioManager.PlaySFX(audioManager.click);
        helpPanel.SetActive(true);
        musicPanel.SetActive(false);
    }

    public void ShowMusicPanel()
    {
        audioManager.PlaySFX(audioManager.click);
        musicPanel.SetActive(true);
        helpPanel.SetActive(false);
    }

    public void HideAllPanels()
    {
        audioManager.PlaySFX(audioManager.click);
        helpPanel.SetActive(false);
        musicPanel.SetActive(false);
    }


}
