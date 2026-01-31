using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public BoxCollider2D doorCollider;
    public BoxCollider2D doorTrigger;
    public SpriteRenderer doorSprite;

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
        }
    }

    //public void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (collider.gameObject.CompareTag("Player"))
    //    {
    //        OpenDoor();
    //    }
    //}

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
        if (!doorSprite.enabled)
        {
            doorSprite.enabled = true;
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
        if (doorSprite.enabled)
        {
            doorSprite.enabled = false;
        }
        isClosed = false;
    }
}
