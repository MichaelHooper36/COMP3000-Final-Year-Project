using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    protected Rigidbody2D rigidBody;

    public int enemyDamage = 1;
    public int playerDamage = 1;

    private GameObject projectileOrigin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.linearVelocity = transform.right * speed;

        // Destroy the projectile after 3 seconds to prevent memory leaks
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        
    }

    public virtual void SetOrigin(GameObject projectileOrigin)
    {
        this.projectileOrigin = projectileOrigin;
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == projectileOrigin)
        {
            return; // Ignore collision with the object that fired the projectile
        }

        if (collider.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Projectile hit ground.");
            Destroy(gameObject);
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
            Destroy(gameObject);
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
                Destroy(gameObject);
        }
    }
}
