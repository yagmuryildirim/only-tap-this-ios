using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaSizer : MonoBehaviour
{
    public float height;
    public float width;

    private void Awake()
    {
        height = Camera.main.orthographicSize * 2;
        width = height * Screen.width / Screen.height;
        transform.localScale = new Vector3(width * 0.8f, height * 0.6f, 1);
    }
}
