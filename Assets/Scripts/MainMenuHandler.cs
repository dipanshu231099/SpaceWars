using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject optionsMenu;
    // Custom function
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Controls()
    {
        // nothing to do
    }

    public void Quit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}