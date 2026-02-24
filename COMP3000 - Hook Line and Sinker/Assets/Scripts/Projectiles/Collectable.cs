using UnityEngine;

public class WormCollectable : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public GameObject projectile;
    public int projectileIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (collider.gameObject.CompareTag("Player"))
        {
            playerMovement = collider.GetComponent<PlayerMovement>();

            if (playerMovement.projectiles[projectileIndex] == projectile)
            {
                GameControl.gameControl.projectiles.Add(projectileIndex);
                GameControl.gameControl.Save();
                GameControl.gameControl.Load();
                playerMovement.ChangeProjectile(projectileIndex);
            }
            Destroy(gameObject);
        }
    }
}
