using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public BoxCollider2D doorCollider;
    public BoxCollider2D doorTrigger;
    public GameObject grid;

    public bool isClosed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isClosed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            CloseTheDoor();
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.movement = 0;
                playerMovement.rigidBody.linearVelocityX = 0;
            }
        }
    }

    public void CloseTheDoor()
    {
        if (!doorCollider.enabled)
        {
            doorCollider.enabled = true;
        }
        if (doorTrigger.enabled)
        {
            doorTrigger.enabled = false;
        }
        if (grid != null)
        {
            grid.SetActive(true);
        }
        isClosed = true;
    }

    public void OpenDoor()
    {
        if (doorCollider.enabled)
        {
            doorCollider.enabled = false;
        }
        if (!doorTrigger.enabled)
        {
            doorTrigger.enabled = true;
        }
        if (grid != null)
        {
            grid.SetActive(false);
        }
        isClosed = false;
    }
}
