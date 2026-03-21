using UnityEngine;

public class ProjectileMovement : Projectile
{
    public Animator animator;
    public LayerMask groundLayer;
    private PlayerMovement playerMovement;
    public ProjectileDamage projectileDamage;
    private bool playerInRange;

    private float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.linearVelocity = transform.right * speed;

        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (playerInRange && playerMovement != null)
        {
            RaycastHit2D hit = Physics2D.Linecast(playerMovement.transform.position, transform.position, groundLayer);

            if (hit.collider == null && !playerMovement.isGrounded)
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

            if (playerMovement.isGrappling && (hit.collider != null || playerMovement.isGrounded || elapsedTime >= 3f))
            {
                playerMovement.isGrappling = false;
                playerMovement.canMove = true;
                playerMovement.canJump = true;
                playerMovement.distanceJoint.enabled = false;
                playerMovement.lineRenderer.enabled = false;
                animator.SetBool("inRange", false);
                playerMovement.canGrapple = false;
                playerMovement.grapplePoint = null;

                if (elapsedTime >=3)
                {
                    playerMovement.animator.SetBool("isGrappling", false);
                    Destroy(gameObject);
                }
            }
            else if (playerMovement.isGrappling)
            {
                animator.SetBool("inRange", false);
                playerMovement.distanceJoint.connectedAnchor = transform.position;
                playerMovement.distanceJoint.distance = Vector2.Distance(transform.position, playerMovement.transform.position);
                playerMovement.rigidBody.linearVelocity = rigidBody.linearVelocity;
            }
        }
    }

    public override void SetOrigin(GameObject projectileOrigin)
    {
        base.SetOrigin(projectileOrigin);

        if (projectileDamage != null)
        {
            projectileDamage.SetOrigin(projectileOrigin);
        }
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerMovement = collider.GetComponent<PlayerMovement>();
            playerInRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && playerMovement != null && !playerMovement.isGrappling)
        {
            playerMovement.grapplePoint = null;
            playerMovement.canGrapple = false;
            animator.SetBool("inRange", false);
            playerInRange = false;
            playerMovement = null;
        }
    }
}
