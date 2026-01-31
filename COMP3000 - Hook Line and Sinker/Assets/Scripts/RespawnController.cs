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
                PlayerPrefs.SetFloat("RespawnX", respawnPoint.position.x);
                PlayerPrefs.SetFloat("RespawnY", respawnPoint.position.y);
                PlayerPrefs.Save();

                respawnColour.color = Color.green;
            }
        }
    }
}
