using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    private bool _bloom;

    void Start()
    {
        CameraUpdate();
    }

    public void CameraUpdate()
    {
        _bloom = SettingsController._isBloom;
        Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = _bloom;
    }

    public bool Bloom()
    {
        return _bloom;
    }
}
