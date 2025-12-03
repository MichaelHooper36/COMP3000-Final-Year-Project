using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public bool hitEnemy = false;
    public bool hitGround = false;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = transform.right * speed;
    }

    void Update()
    {
        hitGround = Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer);
        hitEnemy = Physics2D.OverlapCircle(transform.position, 0.2f, enemyLayer);
        if (hitGround)
        {
            Debug.Log("Projectile hit ground.");
            Destroy(gameObject);
        }
        if (hitEnemy)
        {
            Debug.Log("Projectile hit enemy.");
            Destroy(gameObject);
        }

        // Destroy the projectile after 5 seconds to prevent memory leaks
        Destroy(gameObject, 5f);
    }
}
