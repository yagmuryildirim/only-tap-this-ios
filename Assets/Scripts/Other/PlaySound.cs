using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource winAudio;
    public AudioSource failAudio;
    public AudioSource buttonClickAudio;
    public AudioSource comboAudio;
    public AudioSource fasterAudio;
    public AudioSource levelMusic;

    public bool musicEnabled = false;
    public bool soundFXEnabled = false;

    public void Awake()
    {
        if (PlayerPrefs.GetInt("first", 1) == 1)
        {
            //First time opening game
            PlayerPrefs.SetInt("first", 0);
            PlayerPrefs.SetString("music", "enabled");
            PlayerPrefs.SetString("sfx", "enabled");
            musicEnabled = true;
            soundFXEnabled = true;
        }
        else
        {
            if (PlayerPrefs.GetString("music") == "enabled") musicEnabled = true;
            else musicEnabled = false;
            if (PlayerPrefs.GetString("sfx") == "enabled") soundFXEnabled = true;
            else soundFXEnabled = false;
        }
        if (musicEnabled) levelMusic.Play();
    }

    public void PlayWin()
    {
        if (soundFXEnabled)
            winAudio.Play();
    }
    public void PlayFail()
    {
        if (soundFXEnabled)
            failAudio.Play();
    }

    public void ButtonClickSound()
    {
        if (soundFXEnabled)
            buttonClickAudio.Play();
    }

    public void PlayFasterSound()
    {
        if (soundFXEnabled)
            fasterAudio.Play();
    }
    public void PlayComboSound()
    {
        if (soundFXEnabled)
            comboAudio.Play();
    }
}
