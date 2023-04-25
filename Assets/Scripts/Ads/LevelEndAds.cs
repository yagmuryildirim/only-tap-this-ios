using System.Collections;
using System.Collections.Generic;
using Unity.Advertisement.IosSupport.Samples;
using UnityEngine;

public class LevelEndAds : MonoBehaviour
{
    AdManager adManager;

    private void Awake()
    {
        adManager = FindObjectOfType<AdManager>();
    }

    private void OnDisable()
    {
        adManager.DestroyBanner();
    }
}
