using UnityEngine;

public class RisingFloor : MonoBehaviour
{
    public Boss boss;

    public bool isRising;
    public BoxCollider2D ceilingBody;
    public SpriteRenderer ceilingSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isRising && boss.dead)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 5f * Time.deltaTime, transform.position.z);
        }
        else if (isRising)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1f * Time.deltaTime, transform.position.z);
        }

        if (transform.position.y >= 53f && boss.phaseThree)
        {
            isRising = false;
        }
        else if (transform.position.y >= 28.5f && boss.phaseTwo)
        {
            isRising = false;
        }
        else if (transform.position.y <= 2.5f && boss.dead)
        {
            isRising = false;
        }
    }
}
