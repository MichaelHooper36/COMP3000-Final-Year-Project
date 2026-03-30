using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if !UNITY_EDITOR
using System.Diagnostics;
using System.IO;
#endif
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Sprite chopsticksSprite;
    public Texture2D chopsticksCursor;
    public Image cursorImage;

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
        ChangeCursor(chopsticksSprite, chopsticksCursor);
        if (GameControl.gameControl.device == GameControl.Device.Keyboard)
        {
            Cursor.visible = true;
            cursorImage.gameObject.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
            cursorImage.gameObject.SetActive(true);
        }
        mainMenu.SetActive(true);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCursor(Sprite sprite, Texture2D cursor)
    {
        cursorImage.sprite = sprite;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    public void LevelSelect()
    {
        if (GameControl.gameControl.levelOneTimer != 0)
        {
            level1Continue.SetActive(true);
        }
        if (GameControl.gameControl.levelTwoTimer != 0)
        {
            level2Continue.SetActive(true);
        }
        if (GameControl.gameControl.levelThreeTimer != 0)
        {
            level3Continue.SetActive(true);
        }

        if (GameControl.gameControl.levelOneBest != 0)
        {
            level1Best.SetActive(true);
            TextMeshProUGUI bestTime = level1Best.transform.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();
            if (bestTime != null)
            {
                float elapsedBestTime = GameControl.gameControl.levelOneBest;
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
        GameControl.gameControl.levelOneRespawnX = -16;
        GameControl.gameControl.levelOneRespawnY = 3;
        GameControl.gameControl.levelOneTimer = 0;
        GameControl.gameControl.projectileIndex = 0;
        GameControl.gameControl.projectiles.Remove(1);
        GameControl.gameControl.Save();
        GameControl.gameControl.Load();
        LoadScene("levelOne");
    }

    public void LevelOne()
    {
        LoadScene("levelOne");
    }

    public void NewLevelTwo()
    {
        GameControl.gameControl.levelTwoRespawnX = 0;
        GameControl.gameControl.levelTwoRespawnY = 0;
        GameControl.gameControl.levelTwoTimer = 0;
        GameControl.gameControl.projectileIndex = 0;
        GameControl.gameControl.projectiles.Remove(3);
        GameControl.gameControl.Save();
        GameControl.gameControl.Load();
        LoadScene("levelTwo");
    }

    public void LevelTwo()
    {
        LoadScene("levelTwo");
    }

    public void NewLevelThree()
    {
        GameControl.gameControl.levelThreeRespawnX = 0;
        GameControl.gameControl.levelThreeRespawnY = 0;
        GameControl.gameControl.levelThreeTimer = 0;
        GameControl.gameControl.projectileIndex = 0;
        GameControl.gameControl.Save();
        GameControl.gameControl.Load();
        LoadScene("levelThree");
    }

    public void LevelThree()
    {
        LoadScene("levelThree");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
