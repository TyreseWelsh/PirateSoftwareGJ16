using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
    public string playScene; 
    public GameObject mainMenu;
    public GameObject SettingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }

    public void BackMenu()
    {
        credits.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SettingsMenuF()
    {
        mainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void SettingsBackMenu()
    {
        mainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }
}
