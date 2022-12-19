using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public static SettingsController instance; 

    [SerializeField] private Sprite offAudioButtonSprite;
    [SerializeField] private Sprite onAudioButtonSprite;
    [SerializeField] private GameObject audioButton;
    public static bool playAudio;

    private void Awake()
    {
        if(instance == null)
                instance = this;

        if (!PlayerPrefs.HasKey("Audio"))    
                PlayerPrefs.SetInt("Audio", 0);      
    }

    private void Start()
    {
        PlayerPrefs.HasKey("Audio");
        AudioListener.volume = PlayerPrefs.GetInt("Audio");
        AudioController();
    }
    public void AudioController()
    {
        switch (AudioListener.volume)
        {
            case 0:
                {
                    PlayerPrefs.SetInt("Audio", 0);
                    playAudio = true;
                    AudioListener.volume = 1;
                    audioButton.GetComponent<Image>().sprite = onAudioButtonSprite;                         
                }
                break;

            case 1:
                {
                    PlayerPrefs.SetInt("Audio", 1);
                    playAudio = false;
                    AudioListener.volume = 0;
                    audioButton.GetComponent<Image>().sprite = offAudioButtonSprite;                     
                }
                break;
        }
    }
}
