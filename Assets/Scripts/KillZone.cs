using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController playerController = collision.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.Explode();
        }
    }
}