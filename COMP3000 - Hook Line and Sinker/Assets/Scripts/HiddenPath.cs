using UnityEngine;

public class HiddenPath : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
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

            if (playerMovement.isGrappling && playerMovement.grapplePoint == transform)
            {

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
