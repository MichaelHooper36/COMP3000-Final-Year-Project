using UnityEngine;

public class HiddenPath : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public LayerMask groundLayer;
    private PlayerMovement playerMovement;
    private bool playerInRange;
    private Vector2 originalPosition;
    
    public GameObject tileMap;
    public GameObject[] debris;
    private Disappear disappear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && playerMovement != null)
        {
            RaycastHit2D hit = Physics2D.Linecast(playerMovement.transform.position, transform.position, groundLayer);

            if (hit.collider == null)
            {
                spriteRenderer.color = Color.black;
                playerMovement.canGrapple = true;
                playerMovement.grapplePoint = transform;
            }
            else
            {
                spriteRenderer.color = Color.white;
                playerMovement.canGrapple = false;
                playerMovement.grapplePoint = null;
            }

            if (playerMovement.distanceJoint.enabled)
            {
                spriteRenderer.color = Color.white;
                Rigidbody2D rigidBody = transform.parent.GetComponent<Rigidbody2D>();
                rigidBody.constraints = RigidbodyConstraints2D.None;

                foreach (GameObject debrisPiece in debris)
                {
                    Rigidbody2D debrisRigidBody = debrisPiece.GetComponent<Rigidbody2D>();
                    if (debrisRigidBody != null)
                    {
                        debrisRigidBody.constraints = RigidbodyConstraints2D.None;
                    }
                }

                playerMovement.canMove = true;
                playerMovement.isGrappling = false;
                playerMovement.lineRenderer.SetPosition(0, transform.position);
                playerMovement.distanceJoint.connectedAnchor = transform.position;

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
                    playerMovement.rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
                }
                else
                {
                    playerMovement.rigidBody.constraints = RigidbodyConstraints2D.None;
                    playerMovement.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                }

                if (hit.collider != null)
                {
                    playerMovement.canJump = true;
                    playerMovement.distanceJoint.enabled = false;
                    playerMovement.lineRenderer.enabled = false;
                    spriteRenderer.color = Color.white;
                    playerMovement.canGrapple = false;
                    playerMovement.grapplePoint = null;
                    rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rigidBody.constraints = RigidbodyConstraints2D.FreezePosition;

                    foreach (GameObject debrisPiece in debris)
                    {
                        Rigidbody2D debrisRigidBody = debrisPiece.GetComponent<Rigidbody2D>();
                        if (debrisRigidBody != null)
                        {
                            debrisRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
                        }
                    }
                }
            }
            else
            {
                Rigidbody2D rigidBody = transform.parent.GetComponent<Rigidbody2D>();
                rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                playerMovement.rigidBody.constraints = RigidbodyConstraints2D.None;
                playerMovement.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            if (transform.position.x > originalPosition.x + 1.0f || transform.position.x < originalPosition.x - 1.0f || transform.position.y < originalPosition.y - 1.0f)
            {
                playerMovement.rigidBody.constraints = RigidbodyConstraints2D.None;
                playerMovement.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                playerMovement.isGrappling = false;
                playerMovement.canMove = true;
                playerMovement.canJump = true;
                playerMovement.distanceJoint.enabled = false;
                playerMovement.lineRenderer.enabled = false;
                spriteRenderer.color = Color.white;
                playerMovement.canGrapple = false;
                playerMovement.grapplePoint = null;

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

                Destroy(transform.parent.gameObject);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerInRange = false;
                spriteRenderer.color = Color.white;
                playerMovement = null;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
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
