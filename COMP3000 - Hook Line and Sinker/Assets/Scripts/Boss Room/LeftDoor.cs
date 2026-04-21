using UnityEngine;

public class LeftDoor : MonoBehaviour
{
    public RightDoor rightDoor;
    public BoxCollider2D leftDoorCollider;
    public BoxCollider2D leftDoorTrigger;
    public GameObject leftGrid;
    public bool leftDoorClosed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftDoorClosed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // If the player walk into the left door and the right door is still open, then close the left door
        if (collider.gameObject.CompareTag("Player") && !rightDoor.rightDoorClosed)
        {
            CloseLeftDoor();
        }
    }

    public void CloseLeftDoor()
    {
        // Disables the door trigger and closes the door.
        if (!leftDoorCollider.enabled)
        {
            leftDoorCollider.enabled = true;
        }
        if (leftDoorTrigger.enabled)
        {
            leftDoorTrigger.enabled = false;
        }
        if (!leftGrid.activeInHierarchy)
        {
            leftGrid.SetActive(true);
        }
        leftDoorClosed = true;
    }

    public void OpenleftDoor()
    {
        // Enables the door trigger and opens the door.
        if (leftDoorCollider.enabled)
        {
            leftDoorCollider.enabled = false;
        }
        if (!leftDoorTrigger.enabled)
        {
            leftDoorTrigger.enabled = true;
        }
        if (leftGrid.activeInHierarchy)
        {
            leftGrid.SetActive(false);
        }
        leftDoorClosed = false;
    }
}
