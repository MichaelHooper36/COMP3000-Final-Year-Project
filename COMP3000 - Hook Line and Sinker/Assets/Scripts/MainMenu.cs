#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#else
using System.Diagnostics;
using System.IO;
#endif

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelectMenu;
    public GameObject settingsMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenu.SetActive(true);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelSelect()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
    }

    public void LevelOne()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void LevelThree()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
