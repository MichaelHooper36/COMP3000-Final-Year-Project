using UnityEngine;

public class RisingFloor : MonoBehaviour
{
    public Boss boss;
    public PlayerMovement playerMovement;

    public bool isRising;
    public bool playerAtNextPhase;

    public float phaseOneY = 2.5f;
    public float phaseTwoY = 37f;
    public float phaseThreeY = 61f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If a player reaches the next phase of the boss fight, sets the next phase bool to true.
        if ((boss.phaseTwo && playerMovement.transform.position.y >= phaseTwoY && playerMovement.isGrounded) || (boss.phaseThree && playerMovement.transform.position.y >= phaseThreeY && playerMovement.isGrounded))
        {
            playerAtNextPhase = true;
        }

        // When the boss dies, the floor will fall back down towards it's starting point.
        if (isRising && boss.dead)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 10f * Time.deltaTime);
        }
        // If the floor is rising and the next phase bool is true, the floor will rise at a much faster rate.
        else if (isRising && playerAtNextPhase)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 15f * Time.deltaTime);
        }
        // If the player is far enough away from the floor, it will get faster.
        else if (isRising && playerMovement.transform.position.y - transform.position.y > 12.5)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 2f * Time.deltaTime);
        }
        // if the player is slightly closer to the floor, it will rise at a slower rate.
        else if (isRising && (playerMovement.transform.position.y - transform.position.y > 7.5))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1.5f * Time.deltaTime);
        }
        // if the player is very close to the floor, it will rise at its normal rate.
        else if (isRising)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1f * Time.deltaTime);
        }

        // Phase three position
        if (transform.position.y >= phaseThreeY - 7.5f && boss.phaseThree)
        {
            isRising = false;
        }
        // Phase two position
        else if (transform.position.y >= phaseTwoY - 7.5f && boss.phaseTwo)
        {
            isRising = false;
        }
        // phase one and death position
        else if (transform.position.y <= phaseOneY && boss.dead)
        {
            isRising = false;
        }
    }
}
