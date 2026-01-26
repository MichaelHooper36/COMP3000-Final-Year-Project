using UnityEngine;

public class RisingFloor : MonoBehaviour
{
    public CloseDoor closeDoor;
    public bool isRising;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (closeDoor.isClosed)
        {
            isRising = true;
        }

        if (isRising)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f * Time.deltaTime, transform.position.z);
        }

        if (transform.position.y >= 43f)
        {
            isRising = false;
            closeDoor.OpenDoor();
        }
    }
}
