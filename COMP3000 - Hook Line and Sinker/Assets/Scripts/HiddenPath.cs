using UnityEngine;

public class HiddenPath : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public LayerMask groundLayer;
    private PlayerMovement playerMovement;
    private bool playerInRange;
    private Vector2 originalPosition;

    public GameObject tutorial;
    public GameObject tileMap;
    public GameObject[] debris;
    private Disappear disappear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
        tutorial.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && playerMovement != null)
        {
            // Check if there is a wall in the way.
            RaycastHit2D hit = Physics2D.Linecast(playerMovement.transform.position, transform.position, groundLayer);
            bool blockedByGround = hit.collider != null && !hit.collider.transform.IsChildOf(transform.parent);

            if (!blockedByGround && playerMovement.isGrounded)
            {
                animator.SetBool("inRange", true);
                playerMovement.canGrapple = true;
                playerMovement.grapplePoint = transform;
            }
            else
            {
                animator.SetBool("inRange", false);
                playerMovement.canGrapple = false;
                playerMovement.grapplePoint = null;
            }

            if (playerMovement.distanceJoint.enabled)
            {
                // If player is grappling, allows the wall and debris to move with the player.
                animator.SetBool("inRange", false);
                Rigidbody2D rigidBody = transform.parent.GetComponent<Rigidbody2D>();
                rigidBody.constraints = RigidbodyConstraints2D.None;
                rigidBody.bodyType = RigidbodyType2D.Dynamic;

                foreach (GameObject debrisPiece in debris)
                {
                    Rigidbody2D debrisRigidBody = debrisPiece.GetComponent<Rigidbody2D>();
                    if (debrisRigidBody != null)
                    {
                        debrisRigidBody.constraints = RigidbodyConstraints2D.None;
                        debrisRigidBody.bodyType = RigidbodyType2D.Dynamic;
                    }
                }

                playerMovement.canMove = true;
                playerMovement.isGrappling = false;
                playerMovement.lineRenderer.SetPosition(0, transform.position);
                playerMovement.distanceJoint.connectedAnchor = transform.position;

                // If the player moves, the wall moves.
                if (playerMovement.rigidBody.linearVelocityX > 0 && transform.position.x < playerMovement.transform.position.x)
                {
                    rigidBody.linearVelocityX = 1f;
                }
                else if (playerMovement.rigidBody.linearVelocityX < 0 && transform.position.x > playerMovement.transform.position.x)
                {
                    rigidBody.linearVelocityX = -1f;
                }

                if (playerMovement.isGrounded)
                {
                    playerMovement.rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                }
                else
                {
                    playerMovement.rigidBody.constraints = RigidbodyConstraints2D.None;
                    playerMovement.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                }

                if (blockedByGround)
                {
                    // If a wall is in the way, the player stops grappling and the wall and debris stop moving.
                    playerMovement.canJump = true;
                    playerMovement.distanceJoint.enabled = false;
                    playerMovement.lineRenderer.enabled = false;
                    animator.SetBool("inRange", false);
                    playerMovement.canGrapple = false;
                    playerMovement.grapplePoint = null;
                    rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePosition;
                    playerMovement.animator.SetBool("isGrappling", false);

                    foreach (GameObject debrisPiece in debris)
                    {
                        Rigidbody2D debrisRigidBody = debrisPiece.GetComponent<Rigidbody2D>();
                        if (debrisRigidBody != null)
                        {
                            debrisRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                        }
                    }
                }
            }
            else
            {
                // Stops the wall and debris from moving when the player is not grappling.
                Rigidbody2D rigidBody = transform.parent.GetComponent<Rigidbody2D>();
                rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePosition;
                rigidBody.bodyType = RigidbodyType2D.Kinematic;

                foreach (GameObject debrisPiece in debris)
                {
                    Rigidbody2D debrisRigidBody = debrisPiece.GetComponent<Rigidbody2D>();
                    if (debrisRigidBody != null)
                    {
                        debrisRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                        debrisRigidBody.bodyType = RigidbodyType2D.Kinematic;
                    }
                }

                playerMovement.rigidBody.constraints = RigidbodyConstraints2D.None;
                playerMovement.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            if (transform.position.x > originalPosition.x + 0.5f || transform.position.x < originalPosition.x - 0.5f || transform.position.y < originalPosition.y - 0.5f)
            {
                // If the wall has moved far enough, the player stops grappling.
                playerMovement.rigidBody.constraints = RigidbodyConstraints2D.None;
                playerMovement.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                playerMovement.isGrappling = false;
                playerMovement.canMove = true;
                playerMovement.canJump = true;
                playerMovement.distanceJoint.enabled = false;
                playerMovement.lineRenderer.enabled = false;
                animator.SetBool("inRange", false);
                playerMovement.canGrapple = false;
                playerMovement.grapplePoint = null;
                playerMovement.animator.SetBool("isGrappling", false);

                // Make the wall and debris disappear.
                disappear = tileMap.GetComponent<Disappear>();
                if (disappear != null)
                {
                    disappear.DisappearObject();
                }

                foreach(GameObject debrisPiece in debris)
                {
                    GameObject grid = debrisPiece.transform.Find("Grid")?.gameObject;
                    disappear = grid.GetComponent<Disappear>();
                    if (disappear != null)
                    {
                        disappear.DisappearObject();
                    }
                }

                tutorial.SetActive(true);
                Destroy(transform.parent.gameObject);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        // When the player is out of range, the player cannot grapple.
        if (collider.gameObject.CompareTag("Player"))
        {
            playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerInRange = false;
                playerMovement.grapplePoint = null;
                animator.SetBool("inRange", false);
                playerMovement = null;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // Obtains the player's movement script when in range.
        if (collider.gameObject.CompareTag("Player"))
        {
            playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerInRange = true;
            }
        }
    }
}
