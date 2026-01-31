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

    // Update is called once per frame
    void Update()
    {
        if (closeDoor != null && closeDoor.isClosed && player.transform.position.x >= bossRoom.transform.position.x - 9.5f && player.transform.position.x <= bossRoom.transform.position.x + 9.5f)
        {
            Follow(bossRoom, bossRoomOffset);
        }
        else
        {
            Follow(player, playerOffset);
        }
    }

    void Follow(GameObject gameObject, Vector3 offset)
    {
        Vector3 targetPosition = (gameObject.transform.position + offset);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}

