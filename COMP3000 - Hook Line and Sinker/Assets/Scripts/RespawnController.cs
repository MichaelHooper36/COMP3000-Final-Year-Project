using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class RespawnController : MonoBehaviour
{
    public InputSystem_Actions menuInputs;

    public Animator animator;

    public Transform respawnPoint;

    public GameObject inventoryPoint;
    private TextMeshProUGUI inventoryText;
    public bool inventoryPointActive = false;
    public PauseMenu pauseMenu;

    private PlayerMovement playerMovement;
    private Scene scene;

    void Awake()
    {
        menuInputs = new InputSystem_Actions();
    }

    void OnEnable()
    {
        menuInputs.UI.Enable();
        menuInputs.UI.Upgrade.performed += ToggleUpgradeMenu;
    }

    void OnDisable()
    {
        menuInputs.UI.Disable();
        menuInputs.UI.Upgrade.performed -= ToggleUpgradeMenu;
    }

    void ToggleUpgradeMenu(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryPointActive)
        {
            pauseMenu.playerMovement = playerMovement;
            pauseMenu.Projectile();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Checks the current level and sets the text to the correct device input.
        scene = SceneManager.GetActiveScene();
        inventoryText = inventoryPoint.GetComponentInChildren<TextMeshProUGUI>();
        if (GameControl.gameControl.device == GameControl.Device.Controller)
        {
             if (inventoryText != null)
             {
                 inventoryText.text = "Y";
             }
        }
        else
        {
             if (inventoryText != null)
             {
                 inventoryText.text = "E";
             }
        }
        inventoryPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If this game object is the current respawn point, set the animation to active, otherwise set it to inactive.
        if (scene.name == "levelOne" && respawnPoint.position.x == GameControl.gameControl.levelOneRespawnX && respawnPoint.position.y == GameControl.gameControl.levelOneRespawnY)
        {
            animator.SetBool("isActive", true);
        }
        else
        {
            animator.SetBool("isActive", false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Obtains the player's movement script when in range.
            playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                if (!animator.GetBool("isActive"))
                {
                    // Heals the player if this is a new respawn point.
                    playerMovement.Heal(playerMovement.maxHealth / 2);
                }

                // Sets the respawn point to this location and saves it to the game control script.
                if (scene.name == "levelOne")
                {
                    GameControl.gameControl.levelOneRespawnX = respawnPoint.position.x;
                    GameControl.gameControl.levelOneRespawnY = respawnPoint.position.y;
                    GameControl.gameControl.Save();
                    GameControl.gameControl.Load();
                }
                else if (scene.name == "levelTwo")
                {
                    GameControl.gameControl.levelTwoRespawnX = respawnPoint.position.x;
                    GameControl.gameControl.levelTwoRespawnY = respawnPoint.position.y;
                    GameControl.gameControl.Save();
                    GameControl.gameControl.Load();
                }
                else if (scene.name == "levelThree")
                {
                    GameControl.gameControl.levelThreeRespawnX = respawnPoint.position.x;
                    GameControl.gameControl.levelThreeRespawnY = respawnPoint.position.y;
                    GameControl.gameControl.Save();
                    GameControl.gameControl.Load();
                }
                inventoryPoint.SetActive(true);
                inventoryPointActive = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Sets the player's movement script to null when out of range.
            playerMovement = null;
            inventoryPoint.SetActive(false);
            inventoryPointActive = false;
        }
    }
}
