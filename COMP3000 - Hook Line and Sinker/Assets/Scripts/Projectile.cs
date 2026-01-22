using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    
    public bool hitGround = false;
    public bool hitEnemy = false;
    public bool hitPlayer = false;
    
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public LayerMask playerLayer;

    public int enemyDamage = 1;
    public int playerDamage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = transform.right * speed;

        // Destroy the projectile after 3 seconds to prevent memory leaks
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        hitGround = Physics2D.OverlapCircle(transform.position, 0.15f, groundLayer);
        if (hitGround)
        {
            Debug.Log("Projectile hit ground.");
            Destroy(gameObject);
        }
        
        Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, 0.15f, enemyLayer);
        if (enemyCollider != null)
        {
            Debug.Log("Projectile hit enemy.");
            FishMovement fishMovement = enemyCollider.GetComponent<FishMovement>();
            if (fishMovement != null)
            {
                fishMovement.TakeDamage(enemyDamage);
            }
            Destroy(gameObject);
        }

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, 0.15f, playerLayer);
        if (playerCollider != null)
        {
            Debug.Log("Projectile hit player.");
            PlayerMovement playerMovement = playerCollider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.TakeDamage(playerDamage);
            }
            Destroy(gameObject);
        }
    }
}
