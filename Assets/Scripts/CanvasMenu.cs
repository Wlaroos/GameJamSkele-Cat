using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasMenu : MonoBehaviour
{
    [SerializeField] Button _mainMenuButton;
    [SerializeField] Button _restartButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _resumeButton;
    [SerializeField] PlayerController _playerRef;
    [SerializeField] AudioSource _musicRef;



    private void Awake()
    {
        _musicRef = FindObjectOfType<Music>().GetComponent<AudioSource>();
        HideUI();
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _musicRef = FindObjectOfType<Music>().GetComponent<AudioSource>();
            Time.timeScale = 0f;
            ShowUI();
            _playerRef._paused = true;
            _musicRef.Pause();
        }

    }

    public void ReloadLevel()
    {
        _musicRef.UnPause();
        Time.timeScale = 1f;
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        _musicRef.UnPause();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        HideUI();
        _playerRef._paused = false;
        _musicRef.UnPause();
    }

    public void PlayerDeath()
    {
        ShowUI();
        if (transform.Find("ButtonPanel").Find("Resume") != null)
        {
            transform.Find("ButtonPanel").Find("Resume").gameObject.SetActive(false);
        }
    }

    private void HideUI()
    {
        if (transform.Find("ButtonPanel") != null)
        {
            transform.Find("ButtonPanel").gameObject.SetActive(false);
        }
    }

    private void ShowUI()
    {
        if (transform.Find("ButtonPanel") != null)
        {
            transform.Find("ButtonPanel").gameObject.SetActive(true);
        }
    }

}