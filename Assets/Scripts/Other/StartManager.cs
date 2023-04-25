using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Image startImage;

    private void Awake()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height;
        startImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width * 0.5f);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Main");
    }
}
