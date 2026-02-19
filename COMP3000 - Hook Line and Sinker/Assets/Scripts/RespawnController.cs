using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RespawnController : MonoBehaviour
{
    public InputSystem_Actions menuInputs;

    public Transform respawnPoint;
    public SpriteRenderer respawnColour;

    public GameObject inventoryPoint;
    public bool inventoryPointActive = false;

    private PlayerMovement playerMovement;
    public GameControl gameControl;
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
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (scene.name == "levelOne" && respawnPoint.position.x == gameControl.levelOneRespawnX && respawnPoint.position.y == gameControl.levelOneRespawnY)
        {
            respawnColour.color = Color.green;
        }
        else
        {
            respawnColour.color = Color.white;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                if (scene.name == "levelOne")
                {
                    gameControl.levelOneRespawnX = respawnPoint.position.x;
                    gameControl.levelOneRespawnY = respawnPoint.position.y;
                    gameControl.Save();
                }
            }

            inventoryPoint.SetActive(true);
            inventoryPointActive = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerMovement = null;
            inventoryPoint.SetActive(false);
            inventoryPointActive = false;
        }
    }
}
