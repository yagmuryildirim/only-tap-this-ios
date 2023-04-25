using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonImage : MonoBehaviour
{
    public Button musicButton;
    public Button sfxButton;
    public Sprite musicEnabled;
    public Sprite musicDisabled;
    public Sprite sfxEnabled;
    public Sprite sfxDisabled;

    private void Awake()
    {
        if (PlayerPrefs.GetString("music") == "enabled")
        {
            musicButton.GetComponent<Image>().sprite = musicEnabled;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicDisabled;
        }
        if (PlayerPrefs.GetString("sfx") == "enabled")
        {
            sfxButton.GetComponent<Image>().sprite = sfxEnabled;
        }
        else
        {
            sfxButton.GetComponent<Image>().sprite = sfxDisabled;
        }
    }

    public void ToggleMusic()
    {
        if (PlayerPrefs.GetString("music") == "enabled")
        {
            FindObjectOfType<PlaySound>().musicEnabled = false;
            FindObjectOfType<PlaySound>().levelMusic.Stop();
            PlayerPrefs.SetString("music", "no");
            musicButton.GetComponent<Image>().sprite = musicDisabled;
        }
        else
        {
            FindObjectOfType<PlaySound>().musicEnabled = true;
            FindObjectOfType<PlaySound>().levelMusic.Play();
            PlayerPrefs.SetString("music", "enabled");
            musicButton.GetComponent<Image>().sprite = musicEnabled;
        }
    }
    public void ToggleSFX()
    {
        if (PlayerPrefs.GetString("sfx") == "enabled")
        {
            FindObjectOfType<PlaySound>().soundFXEnabled = false;
            PlayerPrefs.SetString("sfx", "no");
            sfxButton.GetComponent<Image>().sprite = sfxDisabled;
        }
        else
        {
            FindObjectOfType<PlaySound>().soundFXEnabled = true;
            PlayerPrefs.SetString("sfx", "enabled");
            sfxButton.GetComponent<Image>().sprite = sfxEnabled;
        }
    }
}
