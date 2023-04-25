using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleManager : MonoBehaviour
{
    [Header("Locale Button")]
    [SerializeField] private TextMeshProUGUI localeText;
    private string locale = "en";

    private void Start()
    {
        StartCoroutine(SetLocale());
    }

    public void ToggleLocale()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            locale = "tr";
        }
        else if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            locale = "en";
        }
        localeText.text = locale;
        PlayerPrefs.SetString("locale", locale);
    }

    private IEnumerator SetLocale()
    {
        yield return LocalizationSettings.InitializationOperation;
        FindLocale();
        if (locale.Equals("tr")) LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        else if (locale.Equals("en")) LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        PlayerPrefs.SetString("locale", locale);
        if (localeText != null)
            localeText.text = locale;
    }

    private void FindLocale()
    {
        if (PlayerPrefs.GetInt("language", 1) == 1)
        {
            //First time opening app
            if (Application.systemLanguage == SystemLanguage.English)
            {
                locale = "en";
            }
            else if (Application.systemLanguage == SystemLanguage.Turkish)
            {
                locale = "tr";
            }
            else
            {
                locale = "en";
            }
            PlayerPrefs.SetInt("language", 0);
        }
        else
        {
            locale = PlayerPrefs.GetString("locale");
        }
    }
}
