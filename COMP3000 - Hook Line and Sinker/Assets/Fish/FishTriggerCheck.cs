using UnityEngine;

public class FishTriggerCheck : MonoBehaviour
{
    private FishMovement fishMovement;

    void Awake()
    {
        fishMovement = GetComponentInParent<FishMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
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
