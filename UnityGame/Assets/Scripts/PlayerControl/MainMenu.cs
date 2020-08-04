using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, optionsMenu;
    public GameObject firstButton, optionsFirstButton, optionsClosedButton;

    void Start(){
        // set default selected object
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    void Update(){
        if(optionsMenu.activeSelf==true )//&& InputSystem)
        {
            Debug.Log("temp");
            //CloseOptions();
        }
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayerSetup");
    }

    public void OpenOptions(){
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        // deselect selected object from previous menu
        EventSystem.current.SetSelectedGameObject(null);
        // set default selected object
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void CloseOptions(){
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        // deselect selected object from previous menu
        EventSystem.current.SetSelectedGameObject(null);
        // set default selected object
        EventSystem.current.SetSelectedGameObject(optionsClosedButton);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
