using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions inputSystem;

    public float moveSpeed;
    public float jumpSpeed;
    public int extraJumps;
    float movement;
    float previousMovement;
    public bool canMove;
    public bool canJump;

    public bool wallSliding;
    public float wallSlideSpeed;

    public Transform groundCheckTransform;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;
    public Transform wallCheckTransform;
    public float wallCheckRadius;
    public bool isGrounded;

    public bool isWallJumping;
    public float wallJumpDirection;
    public float wallJumpTime;
    public float wallJumpCounter;
    public float wallJumpDuration;
    public Vector2 wallJumpSpeed;

    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    public Transform grapplePoint;
    public bool canGrapple;
    public bool isGrappling;

    Rigidbody2D rigidBody;

    void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputSystem.Player.Enable();
        inputSystem.Player.Move.performed += Movement;
        inputSystem.Player.Jump.performed += Jumping;
        inputSystem.Player.Swing.performed += Grappling;

        inputSystem.Player.Move.canceled += Movement;
        inputSystem.Player.Jump.canceled += Jumping;
        inputSystem.Player.Swing.canceled += Grappling;
    }

    void OnDisable()
    {
        inputSystem.Player.Disable(); 
        inputSystem.Player.Move.performed -= Movement;
        inputSystem.Player.Jump.performed -= Jumping;
        inputSystem.Player.Swing.performed -= Grappling;
    }

    void Movement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>().x;
        previousMovement = movement;
        if (movement < 0 && !isWallJumping)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (movement > 0 && !isWallJumping)
        {
            transform.localScale = new Vector2(1, 1);
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
            else if (extraJumps > 0 && canJump)
            {
                rigidBody.linearVelocityY = jumpSpeed;
                extraJumps--;
            }
            else if (wallJumpCounter > 0f)
            {
                isWallJumping = true;
                movement = (wallJumpDirection * wallJumpSpeed.x) / moveSpeed;
                rigidBody.linearVelocityY = wallJumpSpeed.y;
                wallJumpCounter = 0f;

                if (transform.localScale.x != wallJumpDirection)
                {
                    transform.localScale = new Vector2(1 * wallJumpDirection, 1);
                }

                isWallJumping = false;
            }
        }
    }

    void Grappling(InputAction.CallbackContext context)
    {
        if (context.performed && canGrapple)
        {
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.SetPosition(0, grapplePoint.position);
            distanceJoint.connectedAnchor = grapplePoint.position;
            distanceJoint.distance = Vector2.Distance(transform.position, grapplePoint.position);
            distanceJoint.enabled = true;
            lineRenderer.enabled = true;
            canMove = false;
            canJump = false;
            isGrappling = true;
        }
        else if (context.canceled)
        {
            canMove = true;
            canJump = true;
            isGrappling = false;
            distanceJoint.enabled = false;
            lineRenderer.enabled = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundCheckLayer);
        if (isGrounded)
        {
            extraJumps = 1;
            movement = previousMovement;
        }

        wallSliding = Physics2D.OverlapCircle(wallCheckTransform.position, wallCheckRadius, groundCheckLayer);
        if (wallSliding && !isGrounded && rigidBody.linearVelocityY < 0)
        {
            canJump = false;
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;
            rigidBody.linearVelocityY = -wallSlideSpeed;
        }
        else if (!isGrappling)
        {
            canJump = true;
            wallJumpCounter -= Time.deltaTime;
        }

        if (canMove)
        {
            rigidBody.linearVelocityX = movement * moveSpeed;
        }

        if (distanceJoint.enabled)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
