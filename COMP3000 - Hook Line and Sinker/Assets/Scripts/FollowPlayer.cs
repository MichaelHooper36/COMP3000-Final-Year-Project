using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public CloseDoor closeDoor;
    public GameObject player;
    public GameObject bossRoom;
    public Vector3 playerOffset;
    public Vector3 bossRoomOffset;

    [Range(1, 10)]
    public float smoothSpeed;

    void Start()
    {
        Follow(player, playerOffset);
    }

    // Update is called once per frame
    void Update()
    {
        // If the door to the boss room is closed, the camera is locked to stay a certain distance above the spiked platform.
        if (closeDoor != null && closeDoor.isClosed && player.transform.position.x >= bossRoom.transform.position.x - 9.5f && player.transform.position.x <= bossRoom.transform.position.x + 9.5f)
        {
            Follow(bossRoom, bossRoomOffset);
        }
        // The rest of the time, the camera follows the player at a certain distance.
        else
        {
            Follow(player, playerOffset);
        }
    }

    void Follow(GameObject gameObject, Vector3 offset)
    {
        // The camera moves closer to the target position with an interpolation effect, which changes depending on the smooth speed.
        Vector3 targetPosition = (gameObject.transform.position + offset);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}

