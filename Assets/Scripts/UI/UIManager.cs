using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    public GameObject pausedMenu;
    public TextMeshProUGUI continueButtonText;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Pause()
    {
        if (gameManager.gameMode == GameManager.GameMode.wait)
        {
            pausedMenu.SetActive(false);
        }
        else if (gameManager.gameMode == GameManager.GameMode.play)
        {
            pausedMenu.SetActive(true);
        }
        gameManager.PauseGame();
    }

    public void Continue()
    {
        if (FindObjectOfType<AdManager>() != null)
        {
            FindObjectOfType<AdManager>().UserChoseToWatchAd();
        }
    }

    public void Retry()
    {
        gameManager.RetryGame();
    }
}
