#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#else
using System.Diagnostics;
using System.IO;
#endif
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameControl gameControl;

    public GameObject mainMenu;
    public GameObject levelSelectMenu;
    public GameObject settingsMenu;

    public GameObject level1Continue;
    public GameObject level2Continue;
    public GameObject level3Continue;

    public GameObject level1Best;
    public GameObject level2Best;
    public GameObject level3Best;

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
        if (gameControl.levelOneTimer != 0)
        {
            level1Continue.SetActive(true);
        }
        if (gameControl.levelTwoTimer != 0)
        {
            level2Continue.SetActive(true);
        }
        if (gameControl.levelThreeTimer != 0)
        {
            level3Continue.SetActive(true);
        }

        if (gameControl.levelOneBest != 0)
        {
            Debug.Log("Player has PB");
            level1Best.SetActive(true);
            TextMeshProUGUI bestTime = level1Best.transform.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();
            if (bestTime != null)
            {
                float elapsedBestTime = gameControl.levelOneBest;
                int minutes = Mathf.FloorToInt(elapsedBestTime / 60);
                int seconds = Mathf.FloorToInt(elapsedBestTime % 60);
                bestTime.text = ("Personal Best: " + string.Format("{0:00}:{1:00}", minutes, seconds));
            }
        }

        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
    }

    public void NewLevelOne()
    {
        gameControl.levelOneRespawnX = 0;
        gameControl.levelOneRespawnY = 0;
        gameControl.Save();
        LoadScene("levelOne");
    }

    public void LevelOne()
    {
        LoadScene("levelOne");
    }

    public void NewLevelTwo()
    {
        gameControl.levelTwoRespawnX = 0;
        gameControl.levelTwoRespawnY = 0;
        gameControl.Save();
        LoadScene("Level 2");
    }

    public void LevelTwo()
    {
        LoadScene("Level 2");
    }

    public void NewLevelThree()
    {
        gameControl.levelThreeRespawnX = 0;
        gameControl.levelThreeRespawnY = 0;
        gameControl.Save();
        LoadScene("Level 3");
    }

    public void LevelThree()
    {
        LoadScene("Level 3");
    }

    public void LoadScene(string sceneName)
    {
#if UNITY_EDITOR
        SceneManager.LoadScene(sceneName);
#else
        string executablePath = Path.Combine(Application.dataPath, "COMP3000 - Hook Line and Sinker.exe");
        Process.Start(executablePath, sceneName);
#endif
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
