using UnityEngine;

public class WormCollectable : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public GameObject projectile;
    public int projectileIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If the projectile has already been collected, destroy the collectable.
        if (GameControl.gameControl.projectiles.Contains(projectileIndex))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // If the player picks up the collectable, they equip the projectile and the collectable is destroyed.
        if (collider.gameObject.CompareTag("Player"))
        {
            playerMovement = collider.GetComponent<PlayerMovement>();

            if (playerMovement.projectiles[projectileIndex] == projectile)
            {
                if (!GameControl.gameControl.projectiles.Contains(projectileIndex))
                {
                    GameControl.gameControl.projectiles.Add(projectileIndex);
                }
                playerMovement.ChangeProjectile(projectileIndex);
            }
            Destroy(gameObject);
        }
    }
}
