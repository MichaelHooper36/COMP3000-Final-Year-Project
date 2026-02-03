using System;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    public SpriteRenderer sprintRenderer;
    public LayerMask groundLayer;
    private PlayerMovement playerMovement;
    private bool playerInRange;

    void Awake()
    {
        sprintRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && playerMovement != null)
        {
            RaycastHit2D hit = Physics2D.Linecast(playerMovement.transform.position, transform.position, groundLayer);

            if (hit.collider == null && !playerMovement.isGrounded)
            {
                sprintRenderer.color = Color.black;
                playerMovement.canGrapple = true;
                playerMovement.grapplePoint = transform;
            }
            else
            {
                sprintRenderer.color = Color.white;
                playerMovement.canGrapple = false;
                playerMovement.grapplePoint = null;
            }

            if (playerMovement.isGrappling && (hit.collider != null || playerMovement.isGrounded))
            {
                playerMovement.isGrappling = false;
                playerMovement.canMove = true;
                playerMovement.canJump = true;
                playerMovement.distanceJoint.enabled = false;
                playerMovement.lineRenderer.enabled = false;
                sprintRenderer.color = Color.white;
                playerMovement.canGrapple = false;
                playerMovement.grapplePoint = null;
            }
            else if (playerMovement.isGrappling)
            {
                sprintRenderer.color = Color.white;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
            playerMovement.grapplePoint = null;
            playerMovement.canGrapple = false;
            sprintRenderer.color = Color.white;
            playerInRange = false;
            playerMovement = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            playerMovement = collider.GetComponent<PlayerMovement>();
            playerInRange = true;
        }
    }
}
