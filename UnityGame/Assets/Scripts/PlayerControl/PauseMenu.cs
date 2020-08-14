using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// reference videos
// https://www.youtube.com/watch?v=SXBgBmUcTe0
// https://www.youtube.com/watch?v=JivuXdrIHK0

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject firstButton;

    void Start(){
        pauseMenuUI = this.transform.GetChild(0).gameObject; 
    }
    
    public void PauseSwitch()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        // clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set default selected object
        EventSystem.current.SetSelectedGameObject(firstButton);

        GameIsPaused = true;
    }

    public void LoadOptions()
    {
        Debug.Log("Options Loaded");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        // destroy the player configuration manager and its componenets to reset player setup process
        DontDestroyOnLoadManager.DestroyAll();
        GameIsPaused=false;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
