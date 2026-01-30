using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions inputSystem;

    public int maxHealth;
    public int currentHealth = 0;

    public LayerMask enemyLayer;
    public float enemyDamageCooldown;
    public float enemyDamageTimer;

    public float moveSpeed;
    public float jumpSpeed;
    public int extraJumps;
    public float movement;
    float previousMovement;
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
    public string currentWall;
    public string previousWall;

    public bool isWallJumping;
    public float wallJumpDirection;
    public float wallJumpTime;
    public float wallJumpCounter;
    public Vector2 wallJumpSpeed;

    public Transform firePoint;
    public GameObject projectile;
    public float projectileTimer;
    public float projectileCooldown;

    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    public Transform grapplePoint;
    public bool canGrapple;
    public bool isGrappling;

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
        inputSystem.Player.Jump.performed -= Jumping;
        inputSystem.Player.Swing.performed -= Grappling;
        inputSystem.Player.Attack.performed -= Shooting;
    }

    void Movement(InputAction.CallbackContext context)
    {
        if (context.performed && canMove)
        {
            isMoving = true;
            movement = context.ReadValue<Vector2>().x;
            previousMovement = movement;
            if (movement < 0 && !isWallJumping)
            {
                transform.eulerAngles = new Vector2(0, 180);
            }
            else if (movement > 0 && !isWallJumping)
            {
                transform.eulerAngles = new Vector2(0, 0);
            }
        }
        else if (context.canceled && !isWallJumping)
        {
            movement = 0;
            isMoving = false;
        }
    }
    void Jumping(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded && canJump)
            {
                rigidBody.linearVelocityY = jumpSpeed;
            }
            else if (wallJumpCounter > 0f && currentWall != previousWall)
            {
                previousWall = currentWall;
                isWallJumping = true;
                canMove = false;
                rigidBody.linearVelocityY = wallJumpSpeed.y;
                wallJumpCounter = 0f;
                moveTimer = moveCooldown;

                if (wallJumpDirection == 1)
                {
                    transform.eulerAngles = new Vector2(0, 0);
                    movement = (-wallJumpSpeed.x) / moveSpeed;
                }
                else if (wallJumpDirection == -1)
                {
                    transform.eulerAngles = new Vector2(0, 180);
                    movement = (wallJumpSpeed.x) / moveSpeed;
                }
            }
            else if (extraJumps > 0 && canJump && !wallSliding && !isGrappling)
            {
                rigidBody.linearVelocityY = jumpSpeed;
                extraJumps--;
            }
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
        else if (context.canceled && grapplePoint != null && isGrappling)
        {
            isGrappling = false;
            canMove = true;
            canJump = true;
            distanceJoint.enabled = false;
            lineRenderer.enabled = false;
            previousWall = "";

            if (rigidBody.linearVelocityY > 2)
            {
                rigidBody.linearVelocityY += 1;
            }

            if (Mathf.Abs(rigidBody.linearVelocityX) > Mathf.Abs(previousMovement) * moveSpeed)
            {
                movement = rigidBody.linearVelocityX / moveSpeed;
            }
            else if (Mathf.Abs(rigidBody.linearVelocityX) > .5)
            {
                movement = previousMovement;
            }
            else
            {
                movement = 0;
            }
        }
    }

    void Shooting(InputAction.CallbackContext context)
    {
        if (context.performed && projectileTimer == 0)
        {
            Instantiate(projectile, firePoint.position, firePoint.rotation);
            projectileTimer = projectileCooldown;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        distanceJoint.enabled = false;
        canMove = true;
        canJump = true;
        canGrapple = false;
        isWallJumping = false;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundCheckLayer);
        if (isGrounded && isGrappling)
        {
            isGrappling = false;
        }
        if (isGrounded && isWallJumping)
        {
            isWallJumping = false;
            if (isMoving)
            {
                movement = previousMovement;
            }
        }
        if (isGrounded)
        {
            extraJumps = 1;
            previousWall = "";
            previousGround = transform.position;
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
            }

        }

        Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, 0.6f, enemyLayer);
        if (enemyCollider != null)
        {
            TakeDamage(1);
            rigidBody.linearVelocityY = jumpSpeed;
        }

        wallSliding = Physics2D.OverlapCircle(wallCheckTransform.position, wallCheckRadius, groundCheckLayer);
        if (wallSliding && !isGrounded && !isGrappling && rigidBody.linearVelocityY < 0)
        {
            canJump = false;
            isWallJumping = false;
            if (transform.eulerAngles.y == 180)
            {
                wallJumpDirection = -1;
            }
            else if (transform.eulerAngles.y == 0)
            {
                wallJumpDirection = 1;
            }
            wallJumpCounter = wallJumpTime;
            rigidBody.linearVelocityY = -wallSlideSpeed;
        }
        else if (!isGrappling)
        {
            canJump = true;
            wallJumpCounter -= Time.deltaTime;
        }

        if (!isGrappling)
        {
            rigidBody.linearVelocityX = movement * moveSpeed;
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
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0 && enemyDamageTimer == 0)
        {
            currentHealth -= damage;
            Debug.Log("Player Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Debug.Log("Player Died.");
                Destroy(gameObject);
            }
            enemyDamageTimer = enemyDamageCooldown;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }

    void OnTriggerEnter2D(Collider2D wall)
    {
        if (wall.gameObject.CompareTag("Ground"))
        {
            currentWall = wall.gameObject.name;
        }
    }
}
