using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputSystem_Actions inputSystem;

    public float moveSpeed;
    public float jumpSpeed;
    public int extraJumps = 1;
    float movement;
    public bool canMove;

    public Transform groundCheckTransform;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;
    public bool isGrounded;

    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    public Transform grapplePoint;
    public bool canGrapple;

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
    }

    void Jumping(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                rigidBody.linearVelocityY = jumpSpeed;
            }
            else if (extraJumps > 0)
            {
                rigidBody.linearVelocityY = jumpSpeed;
                extraJumps--;
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
        }
        else if (context.canceled)
        {
            canMove = true;
            distanceJoint.enabled = false;
            lineRenderer.enabled = false;
            //extraJumps++;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        distanceJoint.enabled = false;
        canMove = true;
        canGrapple = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundCheckLayer);
        if (isGrounded)
        {
            extraJumps = 1;
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
