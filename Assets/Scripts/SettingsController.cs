using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public static SettingsController instance;  
    [SerializeField] GameObject onAudioButton;
    [SerializeField] GameObject offAudioButton;
    public static bool playAudio = true;
    private int audioControll = 0 ;

    private void Awake()
    {
           if(instance == null)
            instance = this;


        if (!PlayerPrefs.HasKey("Audio"))
            PlayerPrefs.SetInt("Audio", 0);


    }

    private void Start()
    {
        audioControll = PlayerPrefs.GetInt("Audio");
    }
    /// <summary>
    /// Кнопка ВКЛ Audio
    /// </summary>
    public void OnAudio()
    {           
        playAudio = true;
        offAudioButton.SetActive(false);
        onAudioButton.SetActive(true);          
    }

    /// <summary>
    /// Кнопка ВЫКЛ Audio
    /// </summary>
    public void OffAudio()
    {
        playAudio = false;
        onAudioButton.SetActive(false);
        offAudioButton.SetActive(true);    
    }

    public void AudioController() 
    {
        if (audioControll == 1)
        {
            OnAudio();
            PlayerPrefs.SetInt("Audio", 1);
        }
        else if (audioControll == 0)
        { 
            OffAudio();
            PlayerPrefs.SetInt("Audio", 0);
        }
    }
}
