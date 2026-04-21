using UnityEngine;

public class RightDoor : MonoBehaviour
{
    public LeftDoor leftDoor;
    public BoxCollider2D rightDoorCollider;
    public BoxCollider2D rightDoorTrigger;
    public GameObject rightGrid;
    public bool rightDoorClosed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rightDoorClosed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // If the player walks through the right door and the left door is still open, close the right door.
        if (collider.gameObject.CompareTag("Player") && !leftDoor.leftDoorClosed)
        {
            CloseRightDoor();
        }
    }

    public void CloseRightDoor()
    {
        // Disables the door trigger and closes the door.
        if (!rightDoorCollider.enabled)
        {
            rightDoorCollider.enabled = true;
        }
        if (rightDoorTrigger.enabled)
        {
            rightDoorTrigger.enabled = false;
        }
        if (!rightGrid.activeInHierarchy)
        {
            rightGrid.SetActive(true);
        }
        rightDoorClosed = true;
    }

    public void OpenRightDoor()
    {
        // Enables the door trigger and opens the door.
        if (rightDoorCollider.enabled)
        {
            rightDoorCollider.enabled = false;
        }
        if (!rightDoorTrigger.enabled)
        {
            rightDoorTrigger.enabled = true;
        }
        if (rightGrid.activeInHierarchy)
        {
            rightGrid.SetActive(false);
        }
        rightDoorClosed = false;
    }
}
