using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public Transform respawnPoint;
    public SpriteRenderer respawnColour;

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
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.respawnCoordinates = new Vector2(respawnPoint.position.x, respawnPoint.position.y);
                respawnColour.color = Color.green;
            }
        }
    }
}
