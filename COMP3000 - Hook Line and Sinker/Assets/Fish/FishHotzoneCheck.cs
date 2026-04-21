using UnityEngine;

public class FishHotzoneCheck : MonoBehaviour
{
    private FishMovement fishMovement;
    private bool inRange;

    void Awake()
    {
        // Gets the main movement script from the parent object
        fishMovement = GetComponentInParent<FishMovement>();
    }

    private void Update()
    {
        // Flips the fish to face the player, while the player is in range.
        if (inRange && !fishMovement.attacking)
        {
            fishMovement.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Sets the inRange flag to true when the player enters the trigger area.
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Sets the inRange flag to false when the player exits the trigger area.
        // Calls the SelectTarget function fromm the movement script to make the fish continue swimming around.
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            fishMovement.triggerArea.SetActive(true);
            fishMovement.inRange = false;
            fishMovement.SelectTarget();
        }
    }
}
