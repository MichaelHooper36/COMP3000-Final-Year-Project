using UnityEngine;

public class FishTriggerCheck : MonoBehaviour
{
    private FishMovement fishMovement;

    void Awake()
    {
        // Get the main movement script from the parent object.
        fishMovement = GetComponentInParent<FishMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Sets the inRange flag to true when the player enters the trigger area.
        // Changes the detection area to the larger hot zone and flips the fish to face the player.
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            fishMovement.target = collider.transform;
            fishMovement.inRange = true;
            fishMovement.hotZone.SetActive(true);
            fishMovement.Flip();
        }
    }
}
