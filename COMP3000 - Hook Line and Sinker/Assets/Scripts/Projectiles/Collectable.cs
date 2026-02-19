using UnityEngine;

public class WormCollectable : MonoBehaviour
{
    public GameControl gameControl;
    private PlayerMovement playerMovement;

    public GameObject projectile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

            for (int i = 0; i < playerMovement.projectiles.Count; i++)
            {
                if (playerMovement.projectiles[i] == projectile)
                {
                    gameControl.projectiles.Add(i);
                    playerMovement.ChangeProjectile(i);
                }
            }

            Destroy(gameObject);
        }
    }
}
