using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmojiSpawner : MonoBehaviour
{
    private GameObject tapEmoji;

    [SerializeField] private BoxCollider2D spawnerCollider;
    [SerializeField] private GameObject emojiContainerPrefab;
    [SerializeField] private float spawnInterval = 1f;
    private GameObject emojiContainer;
    private EmojiGroup[] emojiGroups;
    private List<GameObject> currentEmojis;
    private Vector2 boundsMin;
    private Vector2 boundsMax;
    private GameManager gameManager;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        emojiGroups = Resources.LoadAll<EmojiGroup>("");
        InitializeSpawner();
    }

    private void Start()
    {
        boundsMax = spawnerCollider.bounds.max;
        boundsMin = spawnerCollider.bounds.min;
    }

    public void InitializeSpawner()
    {
        SpawnInterval = 1f;
        EmojiGroup currentGroup = emojiGroups[Random.Range(0, emojiGroups.Length)];
        currentEmojis = new List<GameObject>();
        foreach (Transform child in currentGroup.transform)
        {
            currentEmojis.Add(child.gameObject);
        }
        ChooseTapEmoji();
        emojiContainer = Instantiate(emojiContainerPrefab, transform);
        emojiContainer.name = "Emojis";
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEmojiCoroutine());
    }

    public bool CheckIfTapEmoji(GameObject gObject)
    {
        if (gObject.name == string.Format("{0}(Clone)", TapEmoji.name))
        {
            return true;
        }
        else return false;
    }

    private void ChooseTapEmoji()
    {
        int emojiIndex = Random.Range(0, currentEmojis.Count);
        TapEmoji = currentEmojis[emojiIndex];
    }

    private IEnumerator SpawnEmojiCoroutine()
    {
        while (true)
        {
            if (gameManager.gameMode == GameManager.GameMode.play)
            {
                gameManager.ConsecutiveTouches = 0;
                if (gameManager.TimePassed < 15)
                {
                    SpawnEmoji();
                }
                else if (gameManager.TimePassed >= 15 && gameManager.TimePassed < 90)
                {
                    SpawnEmojis((Mathf.FloorToInt(gameManager.TimePassed / 15)) + 1);
                }
                else if (gameManager.TimePassed >= 90)
                {
                    SpawnEmojis(Random.Range(4, 7));
                }
            }
            yield return new WaitForSeconds(SpawnInterval);
        }

    }


    private void SpawnEmoji()
    {
        if (emojiContainer != null)
        {
            int emojiIndex = Random.Range(0, currentEmojis.Count);
            GameObject emoji = Instantiate(currentEmojis[emojiIndex], FindSpawnPosition(), Quaternion.identity);
            emoji.GetComponent<Emoji>().InitializeEmoji(transform, SpawnInterval);
        }
    }

    private void SpawnEmojis(int amount)
    {
        GameObject[] emojis = new GameObject[amount];
        for (int i = 0; i < emojis.Length; i++)
        {
            int emojiIndex = Random.Range(0, currentEmojis.Count);
            Vector2 spawnPoint = FindNonOverlappingSpawnPosition();
            GameObject emoji = Instantiate(currentEmojis[emojiIndex], spawnPoint, Quaternion.identity);
            emoji.GetComponent<Emoji>().InitializeEmoji(transform, SpawnInterval);
        }
    }

    private Vector2 FindNonOverlappingSpawnPosition()
    {
        Vector2 spawnPoint = FindSpawnPosition();
        int iteration = 0;
        while (Physics2D.OverlapCircle(spawnPoint, 1.5f) || iteration < 2)
        {
            spawnPoint = FindSpawnPosition();
            iteration++;
        }
        return spawnPoint;
    }

    private Vector2 FindSpawnPosition()
    {
        float padding = 2f;
        Vector2 newSpawn = new Vector2(Random.Range(boundsMin.x + padding, boundsMax.x - padding), Random.Range(boundsMin.y + padding, boundsMax.y - padding));
        return newSpawn;
    }


    public GameObject TapEmoji { get => tapEmoji; set => tapEmoji = value; }
    public float SpawnInterval { get => spawnInterval; set => spawnInterval = value; }
}