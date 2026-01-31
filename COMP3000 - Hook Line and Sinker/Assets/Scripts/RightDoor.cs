using UnityEngine;

public class RightDoor : MonoBehaviour
{
    public LeftDoor leftDoor;
    public SpriteRenderer rightDoorSprite;
    public BoxCollider2D rightDoorCollider;
    public BoxCollider2D rightDoorTrigger;
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
        if (collider.gameObject.CompareTag("Player") && !leftDoor.leftDoorClosed)
        {
            CloseRightDoor();
        }
    }

    public void CloseRightDoor()
    {
        if (!rightDoorCollider.enabled)
        {
            rightDoorCollider.enabled = true;
        }
        if (rightDoorTrigger.enabled)
        {
            rightDoorTrigger.enabled = false;
        }
        if (!rightDoorSprite.enabled)
        {
            rightDoorSprite.enabled = true;
        }
        rightDoorClosed = true;
    }

    public void OpenRightDoor()
    {
        if (rightDoorCollider.enabled)
        {
            rightDoorCollider.enabled = false;
        }
        if (!rightDoorTrigger.enabled)
        {
            rightDoorTrigger.enabled = true;
        }
        if (rightDoorSprite.enabled)
        {
            rightDoorSprite.enabled = false;
        }
        rightDoorClosed = false;
    }
}
