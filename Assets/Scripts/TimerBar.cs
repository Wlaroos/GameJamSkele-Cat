using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBar : MonoBehaviour
{
    [SerializeField] GameObject _playerRef;
    PlayerController _playerController;
    private Vector3 _offset;

    private void Awake()
    {
        transform.position = new Vector3(_playerRef.transform.position.x -.5f, _playerRef.transform.position.y + 0.5f, _playerRef.transform.position.z);
        _offset = transform.position - _playerRef.transform.position;
        _playerController = _playerRef.GetComponent<PlayerController>();
    }

    private void Update()
    {

        transform.position = _playerRef.transform.position + _offset;

        transform.GetChild(2).GetComponent<Transform>().localScale = new Vector3(((Mathf.Round(_playerController.timeRemaining * 10f) / 10f) + _playerController.timerCount - 1) * (0.16f * (5f / _playerController.maxTimerCount)), 0.1f, 1f);

        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (_playerController._isAlive == true)
            {
                child.SetActive(true);
            }
            else if (_playerController._isAlive == false)
            {
                child.SetActive(false);
            }
        }
    }
}
