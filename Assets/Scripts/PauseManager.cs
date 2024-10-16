using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private GameObject pauseMenu;
    private bool isPaused;

    public bool mainMenu;

    private void Start()
    {
        pauseMenu = GameObject.Find("PauseMenuUI");

        pauseMenu.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (!mainMenu)
        {
            if (isPaused)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(2);
    }
    public void CoopLobby()
    {
        SceneManager.LoadScene(3);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Tutorial()
    {
        SceneManager.LoadScene(1);
    }
}
