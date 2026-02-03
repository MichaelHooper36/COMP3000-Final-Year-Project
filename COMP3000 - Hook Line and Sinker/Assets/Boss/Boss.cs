using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    public CloseDoor closeDoor;
    public RisingFloor risingFloor;

    public int maxHealth;
    public int currentHealth;
    public HealthBar healthBar;

    public bool phaseOne;
    public bool phaseTwo;
    public bool phaseThree;
    public bool dead;

    public float fightTimer;
    public float teleportInterval = 4f;
    public float teleportTimer;

    public GameObject[] phaseOneSpawns;
    public GameObject[] phaseTwoSpawns;
    public GameObject[] phaseThreeSpawns;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
        phaseOne = true;
        phaseOneSpawns = GameObject.FindGameObjectsWithTag("PhaseOneSpawn");
        phaseTwoSpawns = GameObject.FindGameObjectsWithTag("PhaseTwoSpawn");
        phaseThreeSpawns = GameObject.FindGameObjectsWithTag("PhaseThreeSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (closeDoor.isClosed && !dead)
        {
            fightTimer += Time.deltaTime;
            teleportTimer -= Time.deltaTime;

            if (teleportTimer <= 0f)
            {
                Teleport();
                teleportTimer = teleportInterval;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
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
            int randomSpawnTwo = Random.Range(0, phaseOneSpawns.Length);
            transform.position = phaseTwoSpawns[randomSpawnTwo].transform.position;
        }
        else if (currentHealth == maxHealth / 3)
        {
            phaseThree = true;
            phaseTwo = false;
            risingFloor.isRising = true;
            int randomSpawnThree = Random.Range(0, phaseOneSpawns.Length);
            transform.position = phaseThreeSpawns[randomSpawnThree].transform.position;
        }
    }

    public void Teleport()
    {
        if (phaseOne)
        {
            int randomSpawnOne = Random.Range(0, phaseOneSpawns.Length);
            transform.position = phaseOneSpawns[randomSpawnOne].transform.position;
        }
        else if (phaseTwo)
        {
            int randomSpawnTwo = Random.Range(0, phaseTwoSpawns.Length);
            transform.position = phaseTwoSpawns[randomSpawnTwo].transform.position;
        }
        else if (phaseThree)
        {
            int randomSpawnThree = Random.Range(0, phaseThreeSpawns.Length);
            transform.position = phaseThreeSpawns[randomSpawnThree].transform.position;
        }
    }
}
