using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if !UNITY_EDITOR
using System.Diagnostics;
using System.IO;
#endif
using UnityEngine.InputSystem;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public InputSystem_Actions menuInputs;
    public Scene scene;

    public GameObject mainUI;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject projectileMenu;

    public TextMeshProUGUI timer;
    public float elapsedTime;
    public bool timerOn;

    void Awake()
    {
        menuInputs = new InputSystem_Actions();
    }

    void OnEnable()
    {
        menuInputs.UI.Enable();
        menuInputs.UI.Pause.performed += TogglePauseMenu;
    }

    void OnDisable()
    {
        menuInputs.UI.Disable();
        menuInputs.UI.Pause.performed -= TogglePauseMenu;
    }

    void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Pause();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        mainUI.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);

        if (scene.name == "levelOne")
        {
            elapsedTime = GameControl.gameControl.levelOneTimer;
        }
        else if (scene.name == "levelTwo")
        {
            elapsedTime = GameControl.gameControl.levelTwoTimer;
        }
        else if (scene.name == "levelThree")
        {
            elapsedTime= GameControl.gameControl.levelThreeTimer;
        }

        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void Pause()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            projectileMenu.SetActive(false);
        }
    }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Projectile()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            projectileMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            projectileMenu.SetActive(false);
        }
    }

    public void Back()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void MainMenu()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        if (scene.name == "levelOne")
        {
            GameControl.gameControl.levelOneTimer = elapsedTime;
            GameControl.gameControl.Save();
            GameControl.gameControl.Load();
        }
        else if (scene.name == "levelTwo")
        {
            GameControl.gameControl.levelTwoTimer = elapsedTime;
            GameControl.gameControl.Save();
            GameControl.gameControl.Load();
        }
        else if (scene.name == "levelThree")
        {
            GameControl.gameControl.levelThreeTimer = elapsedTime;
            GameControl.gameControl.Save();
            GameControl.gameControl.Load();
        }

#if UNITY_EDITOR
        SceneManager.LoadScene("Title Screen");
#else
        string mainMenuPath = Path.Combine(Application.dataPath, "COMP3000 - Hook Line and Sinker.exe");
        Process.Start(mainMenuPath, "Title Screen");
#endif
    }

    public void QuitGame()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        if (scene.name == "levelOne")
        {
            GameControl.gameControl.levelOneTimer = elapsedTime;
            GameControl.gameControl.Save();
            GameControl.gameControl.Load();
        }
        else if (scene.name == "levelTwo")
        {
            GameControl.gameControl.levelTwoTimer = elapsedTime;
            GameControl.gameControl.Save();
            GameControl.gameControl.Load();
        }
        else if (scene.name == "levelThree")
        {
            GameControl.gameControl.levelThreeTimer = elapsedTime;
            GameControl.gameControl.Save();
            GameControl.gameControl.Load();
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
