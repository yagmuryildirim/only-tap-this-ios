using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoji : MonoBehaviour
{
    public void InitializeEmoji(Transform parent, float destroyTime)
    {
        GetComponent<ParticleSystemRenderer>().material.SetTexture("_MainTex", GetComponent<SpriteRenderer>().sprite.texture);
        transform.parent = parent;
        transform.localScale = Vector3.zero;
        transform.LeanScale(Vector2.one, 0.15f).setEaseInCirc();
        Destroy(gameObject, destroyTime);
    }

    public void PlayParticles()
    {
        GetComponent<ParticleSystem>().Play();
    }
}
