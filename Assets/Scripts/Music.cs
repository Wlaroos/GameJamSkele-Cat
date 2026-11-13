using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]
class Music : MonoBehaviour
{
    private static Music instance = null;
    public static Music Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Music)FindObjectOfType(typeof(Music));
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

    public void UpdateVolume()
    {
        Music.Instance.GetComponent<AudioSource>().volume = SettingsController._musicVolume;
    }

    public void PlaySound(AudioClip clip)
    {
        AudioHelper.PlayClip2D(clip, SettingsController._sfxVolume);
    }

}