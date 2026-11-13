using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]
class PostVolume : MonoBehaviour
{
    private static PostVolume instance = null;
    public static PostVolume Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (PostVolume)FindObjectOfType(typeof(PostVolume));
            }
            return instance;
        }
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

}