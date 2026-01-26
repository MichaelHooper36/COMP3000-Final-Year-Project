using UnityEngine;

public class FishHotzoneCheck : MonoBehaviour
{
    private FishMovement fishMovement;
    private bool inRange;

    void Awake()
    {
        fishMovement = GetComponentInParent<FishMovement>();
    }

    private void Update()
    {
        if (inRange && !fishMovement.attacking)
        {
            fishMovement.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
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
