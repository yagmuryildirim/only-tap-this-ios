using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inGameScoreText;
    [SerializeField] private TextMeshProUGUI levelEndScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject levelEndCanvas;
    [SerializeField] private GameObject onlyTapCanvas;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject notPressedText;
    [SerializeField] private Image onlyTapImage;
    [SerializeField] private GameObject fasterText;
    [SerializeField] private GameObject doubleText;
    [SerializeField] private GameObject tripleText;

    private const float startDelay = 3f;
    private const int maxTimeNotPressed = 20;
    private int score = 0;
    private int highScore;
    private int timePassed = 0;
    private int timeSinceLastTouch = 0;
    private int consecutiveTouches = 0;

    private EmojiSpawner emojiSpawner;
    private AdManager adManager;

    public enum GameMode { wait, play }
    public GameMode gameMode = GameMode.wait;

    public int TimePassed { get => timePassed; set => timePassed = value; }
    public int ConsecutiveTouches { get => consecutiveTouches; set => consecutiveTouches = value; }

    private void Awake()
    {
        emojiSpawner = FindObjectOfType<EmojiSpawner>();
        adManager = FindObjectOfType<AdManager>();
    }

    private void Start()
    {
        StartCoroutine(RecordTime());
        StartCoroutine(StartGame());
        emojiSpawner.StartSpawning();
    }

    private void Update()
    {
        if (gameMode == GameMode.play)
        {
            HandleTouch();
            inGameScoreText.text = score.ToString();
        }

    }

    //Used by Main Menu button
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Start");
    }

    public void RetryGame()
    {
        StartCoroutine(RetryGameCo());
    }

    public void ContinueGame()
    {
        StartCoroutine(ContinueGameCo());
    }

    public void PauseGame()
    {
        if (gameMode == GameMode.wait)
        {
            gameMode = GameMode.play;
        }
        else if (gameMode == GameMode.play)
        {
            gameMode = GameMode.wait;
        }
    }

    private IEnumerator StartGame()
    {
        timeSinceLastTouch = 0;
        inGameScoreText.text = "0";
        onlyTapImage.sprite = emojiSpawner.TapEmoji.GetComponent<SpriteRenderer>().sprite;
        yield return new WaitForSeconds(startDelay);
        onlyTapCanvas.SetActive(false);
        gameMode = GameMode.play;
        pauseButton.SetActive(true);
    }

    private IEnumerator ContinueGameCo()
    {
        timeSinceLastTouch = 0;
        yield return new WaitForSeconds(0.2f);
        levelEndCanvas.SetActive(false);
        gameMode = GameMode.play;
        pauseButton.SetActive(true);
    }

    private IEnumerator RetryGameCo()
    {
        yield return new WaitForSeconds(0.2f);
        TimePassed = 0;
        score = 0;
        Destroy(GameObject.Find("Emojis"));
        emojiSpawner.InitializeSpawner();
        levelEndCanvas.SetActive(false);
        onlyTapCanvas.SetActive(true);
        StartCoroutine(StartGame());
    }

    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D collider2D = Physics2D.OverlapPoint(new Vector2(touchPosition.x, touchPosition.y));
                HandleCollider(collider2D);
            }
        }
    }

    private void HandleCollider(Collider2D collider2D)
    {
        if (collider2D && collider2D.tag == "Emoji")
        {
            if (FindObjectOfType<EmojiSpawner>().CheckIfTapEmoji(collider2D.gameObject))
            {
                //Clicked right emoji
                consecutiveTouches++;
                if (consecutiveTouches == 3)
                {
                    score += 2;
                    StartCoroutine(ShowTripleText());
                    FindObjectOfType<PlaySound>().PlayComboSound();
                }
                if (consecutiveTouches == 2)
                {
                    score += 1;
                    StartCoroutine(ShowComboText());
                    FindObjectOfType<PlaySound>().PlayComboSound();
                }
                Camera.main.GetComponent<PlaySound>().PlayWin();
                StartCoroutine(DestroyEmoji(collider2D.gameObject));
                score++;
                timeSinceLastTouch = 0;
                //Play particle effect
            }
            else
            {
                //Clicked wrong emoji
                //Level stops
                gameMode = GameMode.wait;
                StartCoroutine(LevelEnd());
            }
        }
    }

    private IEnumerator LevelEnd()
    {
        SetScores();
        //Achieved a new high score
        if (highScore == score)
        {
            FindObjectOfType<LeaderboardManager>().ReportScore(score);
        }
        highScoreText.text = "High Score: " + highScore.ToString();
        Camera.main.GetComponent<PlaySound>().PlayFail();
        pauseButton.SetActive(false);
        if (adManager != null)
        {
            adManager.CheckAdCount();
        }
        adManager.RequestBanner();
        yield return new WaitForSeconds(1f);
        levelEndCanvas.SetActive(true);
    }

    private void SetScores()
    {
        var source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
        var scoreText = source["global"]["score"] as StringVariable;
        var highScoreText = source["global"]["highScore"] as StringVariable;
        scoreText.Value = score.ToString();
        highScoreText.Value = CheckHighscore().ToString();
    }

    private IEnumerator NotPressedLevelEnd()
    {
        gameMode = GameMode.wait;
        yield return new WaitForSeconds(0.5f);
        notPressedText.SetActive(true);
        yield return new WaitForSeconds(3f);
        notPressedText.SetActive(false);
        StartCoroutine(LevelEnd());
    }

    private int CheckHighscore()
    {
        if (PlayerPrefs.GetInt(nameof(highScore)) == 0)
        {
            PlayerPrefs.SetInt(nameof(highScore), score);
            return score;
        }
        else if (score > PlayerPrefs.GetInt(nameof(highScore)))
        {
            PlayerPrefs.SetInt(nameof(highScore), score);
            return score;
        }
        else return PlayerPrefs.GetInt(nameof(highScore));
    }

    private IEnumerator RecordTime()
    {
        while (true)
        {
            if (TimePassed % 60 == 0 && TimePassed != 0 && TimePassed <= 360)
            {
                StartCoroutine(ShowFasterText());
                emojiSpawner.SpawnInterval *= 0.92f;
            }
            if (gameMode == GameMode.play)
            {
                TimePassed += 1;
                timeSinceLastTouch += 1;
                if (timeSinceLastTouch > maxTimeNotPressed)
                {
                    timeSinceLastTouch = 0;
                    StartCoroutine(NotPressedLevelEnd());
                }
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private IEnumerator DestroyEmoji(GameObject emoji)
    {
        emoji.GetComponent<SpriteRenderer>().enabled = false;
        emoji.GetComponent<Emoji>().PlayParticles();
        yield return new WaitForSeconds(0.7f);
        Destroy(emoji);
    }

    private IEnumerator ShowFasterText()
    {
        fasterText.SetActive(true);
        FindObjectOfType<PlaySound>().PlayFasterSound();
        yield return new WaitForSeconds(1.8f);
        fasterText.SetActive(false);

    }
    private IEnumerator ShowComboText()
    {
        doubleText.SetActive(true);
        yield return new WaitForSeconds(1f);
        doubleText.SetActive(false);
    }
    private IEnumerator ShowTripleText()
    {
        tripleText.SetActive(true);
        yield return new WaitForSeconds(1f);
        tripleText.SetActive(false);
    }

}
