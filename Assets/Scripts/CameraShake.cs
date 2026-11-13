using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Animator camAnim;
    [SerializeField] private int _camSize = 5;
    [SerializeField] NewScreenShake _newShake;

    public void CamShake()
    {
        StartCoroutine(_newShake.Shake(.15f, (.5f * (_camSize / 5))));
    }

    public void CamShakeReverse()
    {
        StartCoroutine(_newShake.BigShake(.21f, (.5f * (_camSize / 5))));
    }

    private void Start()
    {
        _newShake = Camera.main.GetComponent<NewScreenShake>();
        camAnim.gameObject.GetComponent<Camera>().orthographicSize = _camSize;
        _newShake.CameraSize(_camSize);
    }
}