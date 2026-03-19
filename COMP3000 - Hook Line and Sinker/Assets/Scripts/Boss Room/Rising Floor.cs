using UnityEngine;

public class RisingFloor : MonoBehaviour
{
    public Boss boss;
    public PlayerMovement playerMovement;

    public bool isRising;

    public float phaseOneY = 2.5f;
    public float phaseTwoY = 29f;
    public float phaseThreeY = 52f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isRising && boss.dead)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 10f * Time.deltaTime);
        }
        else if (isRising && ((boss.phaseTwo && playerMovement.transform.position.y >= phaseTwoY && playerMovement.isGrounded) || (boss.phaseThree && playerMovement.transform.position.y >= phaseThreeY && playerMovement.isGrounded)))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 10f * Time.deltaTime);
        }
        else if (isRising && playerMovement.transform.position.y - transform.position.y > 12.5)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 2f * Time.deltaTime);
        }
        else if (isRising && (playerMovement.transform.position.y - transform.position.y > 7.5))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1.5f * Time.deltaTime);
        }
        else if (isRising)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1f * Time.deltaTime);
        }

        if (transform.position.y >= phaseThreeY && boss.phaseThree)
        {
            isRising = false;
        }
        else if (transform.position.y >= phaseTwoY && boss.phaseTwo)
        {
            isRising = false;
        }
        else if (transform.position.y <= phaseOneY && boss.dead)
        {
            isRising = false;
        }
    }
}
