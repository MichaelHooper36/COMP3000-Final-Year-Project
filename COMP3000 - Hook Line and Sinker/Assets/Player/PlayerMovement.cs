using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions inputSystem;
    private Scene scene;

    public int maxHealth;
    public int currentHealth = 0;
    public HealthBar healthBar;
    public Vector2 respawnCoordinates;
    public CloseDoor closeDoor;
    public PauseMenu pauseMenu;

    public LayerMask enemyLayer;
    public float enemyDamageCooldown;
    public float enemyDamageTimer;

    public float moveSpeed;
    public float jumpSpeed;
    public int extraJumps;
    public float movement;
    public bool canMove;
    public bool isMoving;
    public bool canJump;

    public float moveTimer;
    public float moveCooldown;

    public bool wallSliding;
    public float wallSlideSpeed;

    public Transform groundCheckTransform;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;
    public Transform wallCheckTransform;
    public float wallCheckRadius;
    public bool isGrounded;

    public bool isWallJumping;
    public Vector2 wallJumpSpeed;

    public Transform firePoint;
    public GameObject startingProjectile;
    public List<GameObject> projectiles;
    public GameObject equippedProjectile;
    public float projectileTimer;
    public float projectileCooldown;
    public bool isShooting;
    private Vector2 aimingInput;
    private bool usingMouse;

    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    public Transform grapplePoint;
    public bool canGrapple;
    public bool isGrappling;
    public bool reelingIn;

    public Rigidbody2D rigidBody;
    public Vector2 previousGround;

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
    }

    void Movement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isMoving = true;
            movement = context.ReadValue<Vector2>().x;
        }
        else if (context.canceled && !isWallJumping)
        {
            movement = 0;
            isMoving = false;
        }
        else if (context.canceled)
        {
            isMoving = false;
        }
    }

    void Aiming(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            aimingInput = context.ReadValue<Vector2>();
            usingMouse = context.control.device is Pointer;
        }
        if (context.canceled && !usingMouse)
        {
            aimingInput = Vector2.zero;
        }
    }

    void Jumping(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrappling)
            {
                reelingIn = true;
            }
            if (isGrounded && canJump)
            {
                rigidBody.linearVelocityY = jumpSpeed;
            }
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
        if (context.performed && canGrapple && grapplePoint != null && !isGrappling)
        {
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.SetPosition(0, grapplePoint.position);
            distanceJoint.connectedAnchor = grapplePoint.position;
            distanceJoint.distance = Vector2.Distance(transform.position, grapplePoint.position);
            distanceJoint.maxDistanceOnly = true;
            distanceJoint.enabled = true;
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

            if (rigidBody.linearVelocityY > 2)
            {
                rigidBody.linearVelocityY += 1;
            }

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
        if (context.performed)
        {
            isShooting = true;
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(respawnCoordinates);
        scene = SceneManager.GetActiveScene();
        rigidBody = GetComponent<Rigidbody2D>();

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

        Debug.Log(respawnCoordinates);
        transform.position = respawnCoordinates;
        distanceJoint.enabled = false;
        canMove = true;
        canJump = true;
        canGrapple = false;
        isWallJumping = false;
        isShooting = false;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        int projectileIndex = GameControl.gameControl.projectileIndex;
        if (GameControl.gameControl.projectiles.Contains(projectileIndex))
        {
            equippedProjectile = projectiles[projectileIndex];
        }
        else
        {
            equippedProjectile = startingProjectile;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAim();

        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundCheckLayer);
        if (isGrounded && isWallJumping)
        {
            isWallJumping = false;
        }
        if (isGrounded)
        {
            extraJumps = 1;
            previousGround = transform.position;
        }
        if (isGrounded && !isMoving)
        {
            movement = 0;
        }

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

        Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, 0.6f, enemyLayer);
        if (enemyCollider != null)
        {
            TakeDamage(10);
            rigidBody.linearVelocityY = jumpSpeed;
        }

        wallSliding = Physics2D.OverlapCircle(wallCheckTransform.position, wallCheckRadius, groundCheckLayer);
        if (wallSliding && !isGrounded && !isGrappling && rigidBody.linearVelocityY < 0)
        {
            canJump = false;
            isWallJumping = false;
            rigidBody.linearVelocityY = -wallSlideSpeed;
        }
        else if (!isGrappling)
        {
            canJump = true;
        }

        if (reelingIn)
        {
            distanceJoint.distance -= 0.01f;
        }

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

        if (distanceJoint.enabled)
        {
            lineRenderer.SetPosition(1, transform.position);

            if (rigidBody.linearVelocityX > 1)
            {
                transform.eulerAngles = new Vector2(0, 0);
            }
            else if (rigidBody.linearVelocityX < -1)
            {
                transform.eulerAngles = new Vector2(0, 180);
            }
        }

        if (isShooting && projectileTimer == 0)
        {
            GameObject firedProjectile = Instantiate(equippedProjectile, firePoint.position, firePoint.rotation);
            firedProjectile.GetComponent<Projectile>().SetOrigin(gameObject);
            projectileTimer = projectileCooldown;
            Debug.Log("Player fired projectile.");
        }

        if (projectileTimer > 0)
        {
            projectileTimer -= Time.deltaTime;
            if (projectileTimer < 0)
            {
                projectileTimer = 0;
            }
        }

        if (enemyDamageTimer > 0)
        {
            enemyDamageTimer -= Time.deltaTime;
            if (enemyDamageTimer < 0)
            {
                enemyDamageTimer = 0;
            }
        }

        if (isGrappling)
        {
            lineRenderer.SetPosition(0, grapplePoint.position);
        }
    }

    void UpdateAim()
    {
        if (wallSliding)
        {
            return;
        }

        Vector2 aimDirection;

        if(usingMouse)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(aimingInput);
            aimDirection = (mousePosition - (Vector2)transform.position).normalized;
        }
        else
        {
            if (aimingInput.sqrMagnitude < 0.01f)
            {
                return;
            }
            aimDirection = aimingInput.normalized;
        }

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        firePoint.position = (Vector2)transform.position + (aimDirection * 0.85f);
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0 && enemyDamageTimer == 0)
        {
            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);
            Debug.Log("Player Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Debug.Log("Player Died.");

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }

    public void ChangeProjectile(int projectileIndex)
    {
        equippedProjectile = projectiles[projectileIndex];
        GameControl.gameControl.projectileIndex = projectileIndex;
        GameControl.gameControl.Save();
        GameControl.gameControl.Load();
    }
}
