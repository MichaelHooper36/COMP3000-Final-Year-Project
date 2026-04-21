using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if !UNITY_EDITOR
using System.Diagnostics;
using System.IO;
#endif
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.LowLevel;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public InputSystem_Actions menuInputs;
    public VirtualMouseInput virtualMouseInput;

    // Other scripts.
    public Scene scene;
    public PlayerMovement playerMovement;

    // Cursors.
    public Sprite crosshairSprite;
    public Texture2D crosshairCursor;
    public Sprite chopsticksSprite;
    public Texture2D chopsticksCursor;
    public Image cursorImage;
    public GameObject virtualCursor;
    public bool cursorPreviouslyActive;

    // Menus.
    public GameObject mainUI;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    // Projectile menu.
    public ScrollRect scrollRect;
    public GameObject projectileMenu;
    public GameObject projectiles;
    private int previousProjectile;
    public GameObject equippedText;

    // Timer.
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
        menuInputs.UI.Return.performed += Return;
    }

    void OnDisable()
    {
        menuInputs.UI.Disable();
        menuInputs.UI.Pause.performed -= TogglePauseMenu;
        menuInputs.UI.Return.performed -= Return;
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
        if (context.performed)
        {
            Back();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Checks the input device.
        if (GameControl.gameControl.device == GameControl.Device.Keyboard)
        {
            Cursor.visible = true;
            virtualCursor.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
            virtualCursor.SetActive(false);
        }
        ChangerCursor(crosshairSprite);

        // Initialises the menu states.
        mainUI.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        projectileMenu.SetActive(false);

        // Checks the current scene and gets the respective timer value.
        scene = SceneManager.GetActiveScene();
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
        // Increments the timer every second the game is not paused.
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
        // Changing the cursor to the crosshair.
        if (texture == crosshairSprite)
        {
            if (virtualCursor.activeInHierarchy && GameControl.gameControl.device == GameControl.Device.Controller)
            {
                cursorImage.sprite = crosshairSprite;
            }
            else if (GameControl.gameControl.device == GameControl.Device.Keyboard)
            {
                Vector2 cursorHotspot = new Vector2(crosshairCursor.width / 2, crosshairCursor.height / 2);
                Cursor.SetCursor(crosshairCursor, cursorHotspot, CursorMode.Auto);
            }
        }
        // Changing the cursor to the chopsticks.
        else
        {
            if (virtualCursor.activeInHierarchy && GameControl.gameControl.device == GameControl.Device.Controller)
            {
                cursorImage.sprite = chopsticksSprite;
            }
            else if (GameControl.gameControl.device == GameControl.Device.Keyboard)
            {
                Cursor.SetCursor(chopsticksCursor, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    public void Pause()
    {
        // If the game is paussed, unpause it.
        if (Time.timeScale == 0f)
        {
            // If the cursor was not active before pausing, hide it again when unpausing.
            if (cursorPreviouslyActive == false)
            {
                Cursor.visible = false;
            }

            Time.timeScale = 1f;
            // Removes the menus and re-enables player movement.
            ChangerCursor(crosshairSprite);
            if (virtualCursor.activeInHierarchy)
            {
                virtualCursor.SetActive(false);
            }
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            projectileMenu.SetActive(false);
            playerMovement.inputSystem.Player.Enable();
        }
        // If the game is not paused, pause it.
        else
        {
            // If the cursor is active before pausing, ensures it remains active when pausing.
            if (Cursor.visible == true)
            {
                cursorPreviouslyActive = true;
            }
            else
            {
                cursorPreviouslyActive = false;
                if (GameControl.gameControl.device == GameControl.Device.Keyboard)
                {
                    Cursor.visible = true;
                }
            }
            Time.timeScale = 0f;

            // If the player is using a controller, activates the virtual cursor and moves it to the centre of the screen.
            if (GameControl.gameControl.device == GameControl.Device.Controller)
            {
                if (!virtualCursor.activeInHierarchy)
                {
                    virtualCursor.SetActive(true);
                }

                Vector2 centre = new Vector2(Screen.width / 2, Screen.height / 2);
                cursorImage.gameObject.transform.position = centre;

                if (virtualMouseInput != null)
                {
                    InputState.Change(virtualMouseInput.virtualMouse.position, centre);
                    InputState.Change(virtualMouseInput.virtualMouse.delta, Vector2.zero);
                }
            }

            // Displays the pause menu and disables player movement.
            ChangerCursor(chopsticksSprite);
            pauseMenu.SetActive(true);
            playerMovement.inputSystem.Player.Disable();
        }
    }

    public void Settings()
    {
        settingsMenu.SetActive(true);
    }

    public void Projectile()
    {
        // If the projectile menu is active, deactivate it.
        if (Time.timeScale == 0f)
        {
            // If the cursor was not active before pausing, hide it again when closing the projectile menu.
            if (cursorPreviouslyActive == false)
            {
                Cursor.visible = false;
            }

            Time.timeScale = 1f;
            // Removes the menus and re-enables player movement.
            ChangerCursor(crosshairSprite);
            if (virtualCursor.activeInHierarchy)
            {
                virtualCursor.SetActive(false);
            }
            pauseMenu.SetActive(false);
            projectileMenu.SetActive(false);
            playerMovement.inputSystem.Player.Enable();
        }
        // If the projectile menu is not active, activate it.
        else
        {
            //If the cursor is active before projectil-ing, ensures it remains active when projectil-ing.
            if (Cursor.visible == true)
            {
                cursorPreviouslyActive = true;
            }
            else
            {
                cursorPreviouslyActive = false;
                if (GameControl.gameControl.device == GameControl.Device.Keyboard)
                {
                    Cursor.visible = true;
                }
            }
            Time.timeScale = 0f;

            // If the player is using a controller, activates the virtual cursor and moves it to the centre of the screen.
            if (GameControl.gameControl.device == GameControl.Device.Controller)
            {
                if (!virtualCursor.activeInHierarchy)
                {
                    virtualCursor.SetActive(true);
                }

                Vector2 centre = new Vector2(Screen.width / 2, Screen.height / 2);
                cursorImage.gameObject.transform.position = centre;

                if (virtualMouseInput != null)
                {
                    InputState.Change(virtualMouseInput.virtualMouse.position, centre);
                    InputState.Change(virtualMouseInput.virtualMouse.delta, Vector2.zero);
                }
            }

            ChangerCursor(chopsticksSprite);

            // Enables the buttons for the collected projectiles and highlights the currently equipped projectile.
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
            playerMovement.inputSystem.Player.Disable();
        }
    }

    // Changes the equipped projectile, highlights the newly equipped projectile and updates the equipped text.
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

    // Returns to the previously open menu, if applicable, or unpauses the game if the pause menu is currently open.
    public void Back()
    {
        if (settingsMenu.activeInHierarchy)
        {
            settingsMenu.SetActive(false);
        }
        else if (projectileMenu.activeInHierarchy)
        {
            Projectile();
        }
        else if (pauseMenu.activeInHierarchy)
        {
            Pause();
        }
    }

    // Saves the timer value and returns to the main menu.
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

    // Saves the timer value and quits the game.
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
