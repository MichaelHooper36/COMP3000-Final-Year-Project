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
    public PlayerMovement playerMovement;

    public Sprite crosshair;
    public Sprite chopsticks;
    public Image cursorImage;
    public bool cursorPreviouslyActive;

    public GameObject mainUI;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    public ScrollRect scrollRect;
    public GameObject projectileMenu;
    public GameObject projectiles;
    private int previousProjectile;
    public GameObject equippedText;

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

    void Return(InputAction.CallbackContext context)
    {
        if (context.performed && pauseMenu.activeInHierarchy)
        {
            Back();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        ChangerCursor(crosshair);
        mainUI.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        projectileMenu.SetActive(false);

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

    public void ChangerCursor(Sprite texture)
    {
        if (texture == crosshair)
        {
            cursorImage.sprite = crosshair;
        }
        else
        {
            cursorImage.sprite = chopsticks;
        }
    }

    public void Pause()
    {
        if (Time.timeScale == 0f)
        {
            if (cursorPreviouslyActive == false)
            {
                Cursor.visible = false;
            }

            Time.timeScale = 1f;
            ChangerCursor(crosshair);
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            projectileMenu.SetActive(false);
        }
        else
        {
            if (Cursor.visible == true)
            {
                cursorPreviouslyActive = true;
            }
            else
            {
                cursorPreviouslyActive = false;
                Cursor.visible = true;
            }
            Time.timeScale = 0f;
            ChangerCursor(chopsticks);
            pauseMenu.SetActive(true);
        }
    }

    public void Settings()
    {
        settingsMenu.SetActive(true);
    }

    public void Projectile()
    {
        if (Time.timeScale == 0f)
        {
            if (cursorPreviouslyActive == false)
            {
                Cursor.visible = false;
            }

            Time.timeScale = 1f;
            ChangerCursor(crosshair);
            pauseMenu.SetActive(false);
            projectileMenu.SetActive(false);
        }
        else
        {
            if (Cursor.visible == true)
            {
                cursorPreviouslyActive = true;
            }
            else
            {
                cursorPreviouslyActive = false;
                Cursor.visible = true;
            }

            ChangerCursor(chopsticks);
            Time.timeScale = 0f;

            for (int i = 0; i < projectiles.transform.childCount; i++)
            {
                if (GameControl.gameControl.projectiles.Contains(i))
                {
                    projectiles.transform.GetChild(i).gameObject.SetActive(true);
                    if (playerMovement.equippedProjectile == playerMovement.projectiles[i])
                    {
                        projectiles.transform.GetChild(i).GetComponent<Image>().color = Color.green;
                        previousProjectile = i;
                    }
                    else
                    {
                        projectiles.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                    }
                }
                else
                {
                    projectiles.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            scrollRect.verticalNormalizedPosition = 1f;
            projectileMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }

    public void EquipProjectile(int newProjectile)
    {
        if (playerMovement != null && newProjectile != GameControl.gameControl.projectileIndex)
        {
            playerMovement.ChangeProjectile(newProjectile);
            projectiles.transform.GetChild(previousProjectile).GetComponent<Image>().color = Color.white;
            projectiles.transform.GetChild(newProjectile).GetComponent<Image>().color = Color.green;
            previousProjectile = newProjectile;
            switch (newProjectile)
            {
                case 0:
                    equippedText.GetComponent<TextMeshProUGUI>().text = "Equipped: Default";
                    break;
                case 1:
                    equippedText.GetComponent<TextMeshProUGUI>().text = "Equipped: Worm";
                    break;
                case 2:
                    equippedText.GetComponent<TextMeshProUGUI>().text = "Equipped: Heavy Groundbait";
                    break;
                case 3:
                    equippedText.GetComponent<TextMeshProUGUI>().text = "Equipped: Ol' Reliable";
                    break;
                default:
                    equippedText.GetComponent<TextMeshProUGUI>().text = "Equipped: None";
                    break;
            }
        }
    }

    public void Back()
    {
        if (settingsMenu.activeInHierarchy)
        {
            settingsMenu.SetActive(false);
        }
        else if (projectileMenu.activeInHierarchy)
        {
             projectileMenu.SetActive(false);
        }
        else if (pauseMenu.activeInHierarchy)
        {
            Pause();
        }
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

        SceneManager.LoadScene("Title Screen");
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
