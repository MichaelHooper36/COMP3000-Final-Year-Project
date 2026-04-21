using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions inputSystem;
    [SerializeField] public Animator animator;
    private Scene scene;

    public int maxHealth;
    public int currentHealth = 0;
    public HealthBar healthBar;
    public Vector2 respawnCoordinates;
    public CloseDoor closeDoor;
    public PauseMenu pauseMenu;

    // Interaction with enemies and damage invulnerability frames
    public LayerMask enemyLayer;
    public float enemyDamageCooldown;
    public float enemyDamageTimer;

    // Movement and jumping
    public float moveSpeed;
    public float jumpSpeed;
    public int extraJumps;
    public float movement;
    public bool canMove;
    public bool isMoving;
    public bool canJump;

    public float moveTimer;
    public float moveCooldown;

    // Wallsliding
    public bool wallSliding;
    public float wallSlideSpeed;
    public float wallSlideTimer;
    public float wallSlideCooldown;
    public bool hangOnWall;

    // Wall jump and grounded detection
    public Transform groundCheckTransform;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;
    public Transform wallCheckTransform;
    public float wallCheckRadius;
    public LayerMask wallCheckLayer;
    public bool isGrounded;

    public bool isWallJumping;
    public Vector2 wallJumpSpeed;

    // Bait
    public Transform firePoint;
    public GameObject startingProjectile;
    public List<GameObject> projectiles;
    public GameObject equippedProjectile;
    public float projectileTimer;
    public float projectileCooldown;
    public bool isShooting;

    // Determining aim direction and source
    private Vector2 aimingInput;
    public Vector2 lastMouseScreenPos;
    public bool mouseMovedLast;
    public float stickDeadzone = 0.2f;

    // Grappling
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    public Transform rendererPoint;
    public Transform grapplePoint;
    public bool canGrapple;
    public bool isGrappling;
    public bool reelingIn;

    public Rigidbody2D rigidBody;
    public Vector2 previousGround;

    private SpriteRenderer spriteRenderer;
    private Color originalColour;

    void Awake()
    {
        inputSystem = new InputSystem_Actions();
        currentHealth = maxHealth;
    }

    void OnEnable()
    {
        inputSystem.Player.Enable();
        inputSystem.Player.Move.performed += Movement;
        inputSystem.Player.Aim.performed += Aiming;
        inputSystem.Player.Jump.performed += Jumping;
        inputSystem.Player.Swing.performed += Grappling;
        inputSystem.Player.Attack.performed += Shooting;

        inputSystem.Player.Move.canceled += Movement;
        inputSystem.Player.Aim.canceled += Aiming;
        inputSystem.Player.Jump.canceled += Jumping;
        inputSystem.Player.Swing.canceled += Grappling;
        inputSystem.Player.Attack.canceled += Shooting;
    }

    void OnDisable()
    {
        inputSystem.Player.Disable(); 
        inputSystem.Player.Move.performed -= Movement;
        inputSystem.Player.Aim.performed -= Aiming;
        inputSystem.Player.Jump.performed -= Jumping;
        inputSystem.Player.Swing.performed -= Grappling;
        inputSystem.Player.Attack.performed -= Shooting;

        inputSystem.Player.Move.canceled -= Movement;
        inputSystem.Player.Aim.canceled -= Aiming;
        inputSystem.Player.Jump.canceled -= Jumping;
        inputSystem.Player.Swing.canceled -= Grappling;
        inputSystem.Player.Attack.canceled -= Shooting;
    }

    void Movement(InputAction.CallbackContext context)
    {
        // If movement has been inputted, this will determine the direction and speed of the player's movement.
        if (context.performed && Mathf.Abs(rigidBody.linearVelocityX) <= wallJumpSpeed.x)
        {
            isMoving = true;
            animator.SetBool("isMoving", true);
            movement = context.ReadValue<Vector2>().x;
        }
        // If movement is cancelled, player movement will stop and running animation will stop.
        else if (context.canceled && !isWallJumping)
        {
            movement = 0;
            isMoving = false;
            animator.SetBool("isMoving", false);
        }
        else if (context.canceled)
        {
            isMoving = false;
            animator.SetBool("isMoving", false);
        }
    }

    void Aiming(InputAction.CallbackContext context)
    {
        // Determining aiming input for devices that aren't the mouse.
        if (context.performed && GameControl.gameControl.device != GameControl.Device.Keyboard)
        {
            Vector2 input = context.ReadValue<Vector2>();

            if (input.sqrMagnitude >= stickDeadzone * stickDeadzone)
            {
                aimingInput = input;
                mouseMovedLast = false;
                firePoint.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else if (context.canceled)
        {
            aimingInput = Vector2.zero;
            firePoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void Jumping(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // If the player is grappling, they will reel in towards the grapple point when they jump.
            if (isGrappling && grapplePoint != null)
            {
                reelingIn = true;
            }
            // Default jump action.
            if (isGrounded && canJump)
            {
                rigidBody.linearVelocityY = jumpSpeed;
            }
            // If against a wall, the player will perform a wall jump.
            else if (wallSliding)
            {
                isWallJumping = true;
                canMove = false;
                rigidBody.linearVelocityY = wallJumpSpeed.y;
                moveTimer = moveCooldown;

                if (transform.eulerAngles.y == 180)
                {
                    transform.eulerAngles = new Vector2(0, 0);
                    if (!isMoving)
                    {
                        movement = wallJumpSpeed.x / moveSpeed;
                    }                    
                    rigidBody.linearVelocityX = wallJumpSpeed.x;
                }
                else if (transform.eulerAngles.y == 0)
                {
                    transform.eulerAngles = new Vector2(0, 180);
                    if (!isMoving)
                    {
                        movement = -wallJumpSpeed.x / moveSpeed;
                    }
                    rigidBody.linearVelocityX = -wallJumpSpeed.x;
                }
            }
            // Players can perform one additional jump midair.
            else if (extraJumps > 0 && canJump && !wallSliding && !isGrappling)
            {
                rigidBody.linearVelocityY = jumpSpeed;
                extraJumps--;
            }
        }
        else if (context.canceled)
        {
            reelingIn = false;
        }
    }

    void Grappling(InputAction.CallbackContext context)
    {
        // On grapple, a joint will be created between the player and the grapple point.
        // And a rope will be rendered between the grapple point and end of the fishing rod.
        if (context.performed && canGrapple && grapplePoint != null && !isGrappling)
        {
            lineRenderer.SetPosition(1, rendererPoint.position);
            lineRenderer.SetPosition(0, grapplePoint.position);
            distanceJoint.connectedAnchor = grapplePoint.position;
            distanceJoint.distance = Vector2.Distance(transform.position, grapplePoint.position);
            distanceJoint.maxDistanceOnly = true;
            distanceJoint.enabled = true;
            animator.SetBool("isGrappling", true);
            lineRenderer.enabled = true;
            canMove = false;
            canJump = false;
            isGrappling = true;
        }
        else if (context.canceled && grapplePoint != null && distanceJoint.enabled)
        {
            isGrappling = false;
            canMove = true;
            canJump = true;
            distanceJoint.enabled = false;
            lineRenderer.enabled = false;
            animator.SetBool("isGrappling", false);

            if (rigidBody.linearVelocityY > 2)
            {
                rigidBody.linearVelocityY += 1;
            }

            // If the player is going fast enough, they will maintain their momentum after releasing the grapple.
            if (Mathf.Abs(rigidBody.linearVelocityX) > Mathf.Abs(movement) * moveSpeed)
            {
                movement = rigidBody.linearVelocityX / moveSpeed;
            }
            else if (rigidBody.linearVelocityX > .5)
            {
                rigidBody.linearVelocityX += 2;
            }
            else if (rigidBody.linearVelocityX < -.5)
            {
                rigidBody.linearVelocityX -= 2;
            }
        }
    }

    void Shooting(InputAction.CallbackContext context)
    {
        // Allows the player to hold down the shoot button.
        if (context.performed)
        {
            isShooting = true;
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        rigidBody = GetComponent<Rigidbody2D>();

        // Determines spawn location, regardless of level.
        float respawnX = 0;
        float respawnY = 0;
        if (scene.name == "levelOne")
        {
            respawnX = GameControl.gameControl.levelOneRespawnX;
            respawnY = GameControl.gameControl.levelOneRespawnY;
        }
        else if (scene.name == "levelTwo")
        {
            respawnX = GameControl.gameControl.levelTwoRespawnX;
            respawnY = GameControl.gameControl.levelTwoRespawnY;
        }
        else if (scene.name == "levelThree")
        {
            respawnX = GameControl.gameControl.levelThreeRespawnX;
            respawnY = GameControl.gameControl.levelThreeRespawnY;
        }

        respawnCoordinates = new Vector2(respawnX, respawnY);

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColour = spriteRenderer.color;

        // Preemptively setting booleans.
        Debug.Log(respawnCoordinates);
        transform.position = respawnCoordinates;
        distanceJoint.enabled = false;
        canMove = true;
        canJump = true;
        canGrapple = false;
        isWallJumping = false;
        isShooting = false;
        hangOnWall = false;
        wallSlideTimer = wallSlideCooldown;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        // Checking save file to determine which projectile the player has equipped, and equipping it.
        int projectileIndex = GameControl.gameControl.projectileIndex;
        if (GameControl.gameControl.projectiles.Contains(projectileIndex))
        {
            equippedProjectile = projectiles[projectileIndex];
        }
        else
        {
            equippedProjectile = startingProjectile;
        }

        // If there is a mouse on the device, this sets the initial position of the mouse.
        if (GameControl.gameControl.device == GameControl.Device.Keyboard)
        {
            lastMouseScreenPos = Mouse.current.position.ReadValue();
        }
    }

    // Update is called once per frame.
    void Update()
    {
        // Determining whether cursor should be visible based on aim source.
        if (GameControl.gameControl.device != GameControl.Device.Keyboard)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

        // If the mouse has moved since the last frame, update the aim source to mouse and update the last mouse position.
        if (GameControl.gameControl.device == GameControl.Device.Keyboard)
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();

            if ((currentMousePosition - lastMouseScreenPos).sqrMagnitude > 0.0001f)
            {
                mouseMovedLast = true;
                lastMouseScreenPos = currentMousePosition;
            }
        }
        UpdateAim();

        // Grounded check.
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundCheckLayer);
        if (isGrounded && isWallJumping)
        {
            isWallJumping = false;
        }

        if (isGrounded)
        {
            // Resetting extra jumps and the previous ground position for when the player falls into spikes.
            extraJumps = 1;
            previousGround = transform.position;
            animator.SetBool("isGrounded", true);
            if (rigidBody.linearVelocityX > moveSpeed)
            {
                rigidBody.linearVelocityX = moveSpeed;
            }
            else if (rigidBody.linearVelocityX < -moveSpeed)
            {
                rigidBody.linearVelocityX = -moveSpeed;
            }

            if (!isMoving)
            {
                movement = 0;
            }
        }
        else
        {
            animator.SetBool("isGrounded", false);
            // Changing animation based on the upwards velocity while midair.
            if (rigidBody.linearVelocityY > 1)
            {
                animator.SetFloat("yVelocity", 1);
            }
            else if (rigidBody.linearVelocityY < -1)
            {
                animator.SetFloat("yVelocity", -1);
            }
            else
            {
                animator.SetFloat("yVelocity", 0);
            }
        }

        // Stops the player from moving for a brief time after wall jumping.
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer < 0)
            {
                moveTimer = 0;
            }
            if (moveTimer == 0)
            {
                canMove = true;
                isWallJumping = false;
                if (!isMoving)
                {
                    movement = rigidBody.linearVelocityX / moveSpeed;
                }
            }

        }

        // If the player touches an enemy, they will take damage and will be moved upwards slightly.
        Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, 0.6f, enemyLayer);
        if (enemyCollider != null)
        {
            TakeDamage(10);
            rigidBody.linearVelocityY = jumpSpeed;
        }

        // Starts timer from when the player first touches a wall.
        if (hangOnWall)
        {
            wallSlideTimer -= Time.deltaTime;
        }
        // If a player touches a wall and is midair, they will stick to the wall.
        wallSliding = Physics2D.OverlapCircle(wallCheckTransform.position, wallCheckRadius, wallCheckLayer) && !isGrounded;
        if (wallSliding && !isGrappling && rigidBody.linearVelocityY < 2.5)
        {
            hangOnWall = true;
            animator.SetBool("isWallSliding", true);
            canJump = false;
            isWallJumping = false;
            rigidBody.linearVelocityX = 0;

            // If enough time has past while on the wall, the player will begin sliding down the wall.
            if (wallSlideTimer <= 0)
            {
                rigidBody.linearVelocityY = -wallSlideSpeed;
            }
            else
            {
                rigidBody.linearVelocityY = 0;
            }
        }
        else if (!isGrappling && !wallSliding)
        {
            animator.SetBool("isWallSliding", false);
            canJump = true;
            if (hangOnWall)
            {
                wallSlideTimer = wallSlideCooldown;
                hangOnWall = false;
            }
        }

        // The player can reel themselves in while grappling, reducing the distance between them and the grapple point.
        if (reelingIn && grapplePoint != null && (grapplePoint.position - rendererPoint.position).sqrMagnitude > 0.1f)
        {
            distanceJoint.distance -= 4f * Time.deltaTime;
        }

        // Determines the player's speed and direction when not grappling or wall jumping.
        if (!isGrappling && !isWallJumping)
        {
            rigidBody.linearVelocityX = movement * moveSpeed;

            if (rigidBody.linearVelocityX < 0)
            {
                transform.eulerAngles = new Vector2(0, 180);
            }
            else if (rigidBody.linearVelocityX > 0)
            {
                transform.eulerAngles = new Vector2(0, 0);
            }
        }

        if (distanceJoint.enabled && grapplePoint != null)
        {
            // Actively updates the rope to follow the player.
            lineRenderer.SetPosition(1, rendererPoint.position);
            lineRenderer.SetPosition(0, grapplePoint.position);

            if (isGrappling)
            {
                // Rotates the player and changes the animation based on the player's position and direction relative to the grapple point.
                float angle = Mathf.Atan2((grapplePoint.position.y - transform.position.y), (grapplePoint.position.x - transform.position.x)) * Mathf.Rad2Deg;

                if ((rigidBody.linearVelocityX > 1 && transform.position.y <= grapplePoint.position.y) || (rigidBody.linearVelocityX < 0 && transform.position.y > grapplePoint.position.y) || (rigidBody.linearVelocityX == 0 && rigidBody.linearVelocityY < 0 && transform.position.x < grapplePoint.position.x))
                {
                    if (angle < 22.5 && angle >= -22.5)
                    {
                        animator.SetFloat("swingDirection", -1f);
                        transform.eulerAngles = new Vector3(0, 0, angle);
                    }
                    else if (angle <= 67.5 && angle >= 22.5)
                    {
                        animator.SetFloat("swingDirection", 0f);
                        transform.eulerAngles = new Vector3(0, 0, angle - 45f);
                    }
                    else
                    {
                        animator.SetFloat("swingDirection", 1f);
                        transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                    }
                }
                else if ((rigidBody.linearVelocityX < 1 && transform.position.y <= grapplePoint.position.y) || (rigidBody.linearVelocityX > 0 && transform.position.y > grapplePoint.position.y) || (rigidBody.linearVelocityX == 0 && rigidBody.linearVelocityY < 0 && transform.position.x > grapplePoint.position.x))
                {
                    if (angle <= -157.5 || angle > 157.5)
                    {
                        animator.SetFloat("swingDirection", -1f);
                        transform.eulerAngles = new Vector3(0, 180, -(angle + 180));
                    }
                    else if (angle >= 112.5 && angle <= 157.5)
                    {
                        animator.SetFloat("swingDirection", 0f);
                        transform.eulerAngles = new Vector3(0, 180, -(angle - 135f));
                    }
                    else
                    {
                        animator.SetFloat("swingDirection", 1f);
                        transform.eulerAngles = new Vector3(0, 180, -(angle - 90f));
                    }
                }
            }
        }
        else
        {
            // Resets the player's rotation when not grappling.
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }

        // As long as the fire button is held, the player will shoot projectiles in the fire point's direction with a cooldown between each shot.
        if (isShooting && projectileTimer == 0 && !wallSliding && (firePoint.position - transform.position).sqrMagnitude > 0.01f)
        {
            GameObject firedProjectile = Instantiate(equippedProjectile, firePoint.position, firePoint.rotation);
            firedProjectile.GetComponent<Projectile>().SetOrigin(gameObject);
            projectileTimer = projectileCooldown;
            Debug.Log("Player fired projectile.");
        }

        // Resets shooting cooldown timer.
        if (projectileTimer > 0)
        {
            projectileTimer -= Time.deltaTime;
            if (projectileTimer < 0)
            {
                projectileTimer = 0;
            }
        }

        // Resets cooldown timer for when the player can take damage from enemies again.
        if (enemyDamageTimer > 0)
        {
            enemyDamageTimer -= Time.deltaTime;
            if (enemyDamageTimer < 0)
            {
                enemyDamageTimer = 0;
            }
        }
    }

    void UpdateAim()
    {
        if (wallSliding)
        {
            return;
        }

        Vector2 aimDirection = Vector2.zero;

        // If the selected device is a keyboard, the fire point will be rotated to face the mouse cursor.
        if (GameControl.gameControl.device == GameControl.Device.Keyboard)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(lastMouseScreenPos);
            aimDirection = (mouseWorldPosition - transform.position).normalized;
        }
        // If the selected device isn't a keyboard, the fire point will be rotated based on the aiming input.
        else
        {
            aimDirection = aimingInput.normalized;
        }

        if (aimDirection.sqrMagnitude < 0.0001f)
        {
            firePoint.rotation = transform.rotation;
            firePoint.position = Vector2.zero;
        }
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
        firePoint.position = (Vector2)transform.position + aimDirection * 0.85f;
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0 && enemyDamageTimer == 0)
        {
            // Health is reduced and a flash of red will play to show the player has been hurt.
            currentHealth -= damage;
            StartCoroutine(DamageFlash());
            healthBar.SetCurrentHealth(currentHealth);
            Debug.Log("Player Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                // If the player dies, the current scene will be reloaded and the player will respawn at the last respawn point.
                if (scene.name == "levelOne")
                {
                    GameControl.gameControl.levelOneTimer = pauseMenu.elapsedTime;
                    GameControl.gameControl.Save();
                    GameControl.gameControl.Load();
                }
                else if (scene.name == "levelTwo")
                {
                    GameControl.gameControl.levelTwoTimer = pauseMenu.elapsedTime;
                    GameControl.gameControl.Save();
                    GameControl.gameControl.Load();
                }
                else if (scene.name == "levelThree")
                {
                    GameControl.gameControl.levelThreeTimer = pauseMenu.elapsedTime;
                    GameControl.gameControl.Save();
                    GameControl.gameControl.Load();
                }

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            enemyDamageTimer = enemyDamageCooldown;
        }
    }

    public void Heal(int healAmount)
    {
        // Heals the player.
        if (healAmount > 0)
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetCurrentHealth(currentHealth);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }

    // Changes the player's equipped projectile from the list of projectiles based on the index given.
    public void ChangeProjectile(int projectileIndex)
    {
        equippedProjectile = projectiles[projectileIndex];
        GameControl.gameControl.projectileIndex = projectileIndex;
        GameControl.gameControl.Save();
        GameControl.gameControl.Load();
    }

    // Flashes the player's sprite red twice.
    private IEnumerator DamageFlash()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = originalColour;
            yield return new WaitForSeconds(0.125f);
        }
    }
}
