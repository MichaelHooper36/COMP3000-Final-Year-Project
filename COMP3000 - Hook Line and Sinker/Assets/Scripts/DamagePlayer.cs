using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public GameObject player;
    public int damage;
    public PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            if (playerMovement != null)
            {
                playerMovement.TakeDamage(damage);
            }
        }
    }
}
