using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingCanvas : MonoBehaviour
{
    AudioSource _musicRef;
    PostVolume _postVolRef;
    private Slider _volumeSlider;
    private Text _volumeText;
    private Slider _sfxVolumeSlider;
    private Text _sfxVolumeText;

    void Start()
    {
        _musicRef = FindObjectOfType<Music>().GetComponent<AudioSource>();
        _postVolRef = FindObjectOfType<PostVolume>();
        _volumeSlider = transform.Find("Panel").Find("VolumeSlider").GetComponent<Slider>();
        _volumeText = transform.Find("Panel").Find("VolumeText").GetComponent<Text>();
        _volumeSlider.value = SettingsController._musicVolume;
        _volumeText.text = "Music Volume: " + Mathf.Round(SettingsController._musicVolume * 100);

        _sfxVolumeSlider = transform.Find("Panel").Find("SFXVolumeSlider").GetComponent<Slider>();
        _sfxVolumeText = transform.Find("Panel").Find("SFXVolumeText").GetComponent<Text>();
        _sfxVolumeSlider.value = SettingsController._sfxVolume;
        _sfxVolumeText.text = "SFX Volume: " + Mathf.Round(SettingsController._sfxVolume * 100);
    }

    private void Awake()
    {

    }

    public void EnableBloom()
    {
        SettingsController._isBloom = true;
        Camera.main.GetComponent<CameraController>().CameraUpdate();
    }

    public void DisableBloom()
    {
        SettingsController._isBloom = false;
        Camera.main.GetComponent<CameraController>().CameraUpdate();
    }

    public void VolumeChange()
    {
        SettingsController._musicVolume = _volumeSlider.value;

        _musicRef.GetComponent<Music>().UpdateVolume();
        _volumeText.text = "Music Volume: " + Mathf.Round(_volumeSlider.value * 100);
    }

    public void SFXVolumeChange()
    {
        SettingsController._sfxVolume = _sfxVolumeSlider.value;
        _sfxVolumeText.text = "SFX Volume: " + Mathf.Round(_sfxVolumeSlider.value * 100);
    }
}
