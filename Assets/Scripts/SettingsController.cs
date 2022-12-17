﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [SerializeField] GameObject onAudioButton;
    [SerializeField] GameObject offAudioButton;

    /// <summary>
    /// Кнопка ВКЛ Audio
    /// </summary>
    public void OnAudio()
    {
        offAudioButton.SetActive(false);
        onAudioButton.SetActive(true);
    }

    /// <summary>
    /// Кнопка ВЫКЛ Audio
    /// </summary>
    public void OffAudio()
    {
        onAudioButton.SetActive(false);
        offAudioButton.SetActive(true);
    }
}
