using System.Collections;
using System.Collections.Generic;
using Unity.Advertisement.IosSupport.Components;
using UnityEngine;

public class RequestTracking : MonoBehaviour
{

    [SerializeField] ContextScreenView contextScreenView;

    private void Start()
    {
        contextScreenView.RequestAuthorizationTracking();
    }
}
