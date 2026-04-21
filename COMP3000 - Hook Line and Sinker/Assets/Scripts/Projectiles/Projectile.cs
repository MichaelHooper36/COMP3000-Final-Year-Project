using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    protected Rigidbody2D rigidBody;

    public int enemyDamage = 10;
    public int playerDamage = 10;

    protected GameObject projectileOrigin;

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

    // Determines what object fired the projectile.
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
        // If the projectile hits the ground, destroy the projectile
        if (collider.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collider.gameObject.CompareTag("Enemy"))
        {
            // If the projectile hits an enemy, deal damage to the enemy and destroy the projectile
            FishMovement fishMovement = collider.GetComponent<FishMovement>();
            if (fishMovement != null)
            {
                fishMovement.TakeDamage(enemyDamage);
                // If the player damaged the enemy, the player becomes the enemy target.
                if (fishMovement.triggerArea.activeInHierarchy && projectileOrigin.CompareTag("Player"))
                {
                    fishMovement.triggerArea.SetActive(false);
                    fishMovement.target = projectileOrigin.transform;
                    fishMovement.inRange = true;
                    fishMovement.hotZone.SetActive(true);
                    fishMovement.Flip();
                }
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
        // If the projectile hits the player, deal damage to the player and destroy the projectile
        else if (collider.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.TakeDamage(playerDamage);
            }
            Destroy(gameObject);
        }
        // If the projectile hits a mine, trigger the mine's explosion animation and destroy the projectile
        else if (collider.gameObject.CompareTag("Mine"))
        {
            Mine mine = collider.GetComponent<Mine>();
            if (mine != null)
            {
                mine.mineAnim.SetBool("isDestroyed", true);
            }
            Destroy(gameObject);
        }
    }
}
