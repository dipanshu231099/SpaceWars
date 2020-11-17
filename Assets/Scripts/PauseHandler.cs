using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{

    public GameObject pauseMenu;
    public bool gamePaused;

    public AudioSource mainGameSource;
    public AudioSource menuAudioSource;

    private bool first;
    private GameObject scoreBoard;
    // Start is called before the first frame update
    void Start()
    {
        first = true;
        scoreBoard = GameObject.Find("Canvas");
        gamePaused = false;
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        scoreBoard.SetActive(false);
        mainGameSource.Pause() ;
        if (!first)
        {
            menuAudioSource.UnPause();
            first = false;  
        }
        else menuAudioSource.Play();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        mainGameSource.UnPause();
        menuAudioSource.Pause();
    }

    public void BackToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        gamePaused = Input.GetKeyDown(KeyCode.Escape);

        if (gamePaused)
        {
            Pause();
        }
    }
}
