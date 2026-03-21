using UnityEngine;

public class ProjectileDamage : Projectile
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == projectileOrigin)
        {
            return; // Ignore collision with the object that fired the projectile
        }

        if (collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Projectile hit ground.");
            PlayerMovement playerMovement = projectileOrigin.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.isGrappling = false;
                playerMovement.canMove = true;
                playerMovement.canJump = true;
                playerMovement.distanceJoint.enabled = false;
                playerMovement.lineRenderer.enabled = false;
                playerMovement.canGrapple = false;
                playerMovement.grapplePoint = null;
            }
            Destroy(transform.parent.gameObject);
        }
        else if (collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Projectile hit enemy.");
            FishMovement fishMovement = collider.GetComponent<FishMovement>();
            if (fishMovement != null)
            {
                fishMovement.TakeDamage(enemyDamage);
            }
            else
            {
                Boss boss = collider.GetComponent<Boss>();
                if (boss != null)
                {
                    boss.TakeDamage(enemyDamage);
                }
            }
            Destroy(transform.parent.gameObject);
        }
        else if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Projectile hit player.");
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                Debug.Log("Projectile dealing damage to player.");
                playerMovement.TakeDamage(playerDamage);
            }
            Destroy(transform.parent.gameObject);
        }
        else if (collider.gameObject.CompareTag("Mine"))
        {
            Debug.Log("Projectile hit mine.");
            Mine mine = collider.GetComponent<Mine>();
            if (mine != null)
            {
                mine.mineAnim.SetBool("isDestroyed", true);
            }
        }
    }
}
