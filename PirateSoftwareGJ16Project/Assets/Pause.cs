using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField]private GameObject playerObject;
    [SerializeField]private GameObject pauseMenuGameObject;
    [SerializeField] private GameObject settingsMenuObject;
    [SerializeField] private GameObject CreditsMenu;
    private MainPlayerController playerControllerRef;
    // Start is called before the first frame update
    void Start()
    {
        playerControllerRef = playerObject.GetComponent<MainPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        
        playerControllerRef.unPause();
    }

    public void Credits()
    {
        CreditsMenu.SetActive(true);
        pauseMenuGameObject.SetActive(false);
    }

    public void backCredits()
    {
        CreditsMenu.SetActive(false);
        pauseMenuGameObject.SetActive(true);
    }

    public void settings()
    {
        settingsMenuObject.SetActive(true);
        pauseMenuGameObject.SetActive(false);
    }

    public void backSettings()
    {
        settingsMenuObject.SetActive(false);
        pauseMenuGameObject.SetActive(true);
    }

    public void exitGame()
    {
        SceneManager.LoadScene(2);
    }
}
