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
        if (PlayerPrefs.HasKey("Level 1 timer"))
        {
            level1Continue.SetActive(true);
        }
        if (PlayerPrefs.HasKey("Level 2 timer"))
        {
            level2Continue.SetActive(true);
        }
        if (PlayerPrefs.HasKey("Level 3 timer"))
        {
            level3Continue.SetActive(true);
        }

        if (PlayerPrefs.HasKey("Level 1 PB"))
        {
            Debug.Log("Player has PB");
            level1Best.SetActive(true);
            TextMeshProUGUI bestTime = level1Best.transform.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();
            if (bestTime != null)
            {
                float elapsedBestTime = PlayerPrefs.GetFloat("Level 1 PB");
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
        PlayerPrefs.DeleteAll();
        LoadScene("Level 1");
    }

    public void LevelOne()
    {
        LoadScene("Level 1");
    }

    public void NewLevelTwo()
    {
        PlayerPrefs.DeleteAll();
        LoadScene("Level 2");
    }

    public void LevelTwo()
    {
        LoadScene("Level 2");
    }

    public void NewLevelThree()
    {
        PlayerPrefs.DeleteAll();
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
