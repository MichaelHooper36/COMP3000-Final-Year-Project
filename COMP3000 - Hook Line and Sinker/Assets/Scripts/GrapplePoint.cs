using System;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public LayerMask groundLayer;
    private PlayerMovement playerMovement;
    private bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && playerMovement != null)
        {
            // Checks if there is a wall between the player and the grapple point.
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
                if (playerMovement.grapplePoint == transform)
                {
                    playerMovement.canGrapple = false;
                    playerMovement.grapplePoint = null;
                }
            }

            if (playerMovement.isGrappling && (hit.collider != null || playerMovement.isGrounded))
            {
                // If there is a wall in the way while grappling, the player stops grappling.
                playerMovement.isGrappling = false;
                playerMovement.canMove = true;
                playerMovement.canJump = true;
                playerMovement.distanceJoint.enabled = false;
                playerMovement.lineRenderer.enabled = false;
                animator.SetBool("inRange", false);
                playerMovement.canGrapple = false;
                playerMovement.grapplePoint = null;
                playerMovement.animator.SetBool("isGrappling", false);
            }
            else if (playerMovement.isGrappling)
            {
                animator.SetBool("inRange", false);
                playerMovement.grapplePoint = transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Resets the player's grapple point and ability to grapple when exiting the area.
        if (collider.name == "Player")
        {
            playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.grapplePoint = null;
                playerMovement.canGrapple = false;
                animator.SetBool("inRange", false);
                playerInRange = false;
                playerMovement = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Obtains the player's movement script when entering the area.
        if (collider.name == "Player")
        {
            playerMovement = collider.GetComponent<PlayerMovement>();
            playerInRange = true;
        }
    }
}
