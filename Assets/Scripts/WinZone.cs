using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    [SerializeField] AudioClip _winSFX01;
    [SerializeField] PlayerController _playerRef;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController playerController = collision.GetComponent<PlayerController>();

        if (playerController != null && playerController._isAlive == true)
        {
            DelayHelper.DelayAction(this, NextScene, 1f);
            AudioHelper.PlayClip2D(_winSFX01, 1f);
            playerController.NextLevel();
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
