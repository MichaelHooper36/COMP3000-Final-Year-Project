using UnityEngine;

public class LeftDoor : MonoBehaviour
{
    public RightDoor rightDoor;
    public SpriteRenderer leftDoorSprite;
    public BoxCollider2D leftDoorCollider;
    public BoxCollider2D leftDoorTrigger;
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
        if (collider.gameObject.CompareTag("Player") && !rightDoor.rightDoorClosed)
        {
            CloseLeftDoor();
        }
    }

    public void CloseLeftDoor()
    {
        if (!leftDoorCollider.enabled)
        {
            leftDoorCollider.enabled = true;
        }
        if (leftDoorTrigger.enabled)
        {
            leftDoorTrigger.enabled = false;
        }
        if (!leftDoorSprite.enabled)
        {
            leftDoorSprite.enabled = true;
        }
        leftDoorClosed = true;
    }

    public void OpenleftDoor()
    {
        if (leftDoorCollider.enabled)
        {
            leftDoorCollider.enabled = false;
        }
        if (!leftDoorTrigger.enabled)
        {
            leftDoorTrigger.enabled = true;
        }
        if (leftDoorSprite.enabled)
        {
            leftDoorSprite.enabled = false;
        }
        leftDoorClosed = false;
    }
}
