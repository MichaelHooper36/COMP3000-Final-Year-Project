using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    public CloseDoor closeDoor;
    public RisingFloor risingFloor;
    public Transform phaseTwoSpawn;
    public Transform phaseThreeSpawn;

    public int maxHealth;
    public int currentHealth;

    public bool phaseOne;
    public bool phaseTwo;
    public bool phaseThree;
    public bool dead;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        phaseOne = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            phaseThree = false;
            dead = true;
            risingFloor.ceilingBody.enabled = false;
            risingFloor.ceilingSprite.enabled = false;
            risingFloor.isRising = true;
            closeDoor.OpenDoor();
            Destroy(closeDoor.gameObject);
            Destroy(gameObject);
        }
        else if (currentHealth == maxHealth * 2 / 3)
        {
            phaseOne = false;
            phaseTwo = true;
            risingFloor.isRising = true;
            transform.position = phaseTwoSpawn.position;
        }
        else if (currentHealth == maxHealth / 3)
        {
            phaseThree = true;
            phaseTwo = false;
            risingFloor.isRising = true;
            transform.position = phaseThreeSpawn.position;
        }
    }
}
