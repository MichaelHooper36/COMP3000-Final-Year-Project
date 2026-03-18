using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    public CloseDoor closeDoor;
    public RisingFloor risingFloor;
    public PauseMenu pauseMenu;

    public Animator animator;

    public GameObject player;
    public GameObject mine;
    public GameObject[] phaseOneMineLocations;
    public GameObject[] phaseTwoMineLocations;
    public GameObject[] phaseThreeMineLocations;

    public int maxHealth;
    public int currentHealth;
    public GameObject bossHealth;
    public HealthBar healthBar;

    public bool phaseOne;
    public bool phaseTwo;
    public bool phaseThree;
    public bool dead;

    public float teleportInterval = 4f;
    public float teleportTimer;
    public bool attacking;
    public float disappearDuration = 1.5f;
    public float reappearDuration = 1.5f;

    public GameObject[] phaseOneSpawns;
    public GameObject phaseOneAttackZone;
    public GameObject[] phaseTwoSpawns;
    public GameObject phaseTwoAttackZone;
    public GameObject[] phaseThreeSpawns;
    public GameObject phaseThreeAttackZone;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        bossHealth.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        phaseOne = true;
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (closeDoor.isClosed && !dead)
        {
            bossHealth.SetActive(true);
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            if (!attacking)
            {
                teleportTimer -= Time.deltaTime;

                if (teleportTimer <= 0f)
                {
                    if (!risingFloor.isRising)
                    {
                        attacking = true;
                        StartCoroutine(MineSpawner());
                    }
                    teleportTimer = teleportInterval;
                }
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
            pauseMenu.timerOn = false;
            GameControl.gameControl.levelOneTimer = pauseMenu.elapsedTime;
            GameControl.gameControl.Save();
            bossHealth.SetActive(false);
            risingFloor.isRising = true;
            closeDoor.OpenDoor();
            Destroy(closeDoor.gameObject);
            Destroy(gameObject);
        }
        else if (currentHealth <= maxHealth / 3)
        {
            phaseThree = true;
            phaseTwo = false;
            risingFloor.isRising = true;
            StartCoroutine(PhaseTransition(phaseThreeAttackZone.transform));
        }
        else if (currentHealth <= maxHealth * 2 / 3)
        {
            phaseOne = false;
            phaseTwo = true;
            risingFloor.isRising = true;
            StartCoroutine(PhaseTransition(phaseTwoAttackZone.transform));
        }
    }

    private IEnumerator PhaseTransition(Transform newPosition)
    {
        animator.SetTrigger("disappear");
        yield return new WaitForSeconds(disappearDuration);
        
        transform.position = newPosition.position;

        animator.SetTrigger("reappear");
        yield return new WaitForSeconds(reappearDuration);

        animator.SetTrigger("idle");
    }

    private IEnumerator MineSpawner()
    {
        if (phaseOne && transform.position != phaseOneAttackZone.transform.position)
        {
            animator.SetTrigger("disappear");
            yield return new WaitForSeconds(disappearDuration);

            transform.position = phaseOneAttackZone.transform.position;

            animator.SetTrigger("reappear");
            yield return new WaitForSeconds(reappearDuration);
        }
        else if (phaseTwo && transform.position != phaseTwoAttackZone.transform.position)
        {
            animator.SetTrigger("disappear");
            yield return new WaitForSeconds(disappearDuration);

            transform.position = phaseTwoAttackZone.transform.position;

            animator.SetTrigger("reappear");
            yield return new WaitForSeconds(reappearDuration);
        }
        else if (phaseThree && transform.position != phaseThreeAttackZone.transform.position)
        {
            animator.SetTrigger("disappear");
            yield return new WaitForSeconds(disappearDuration);

            transform.position = phaseThreeAttackZone.transform.position;

            animator.SetTrigger("reappear");
            yield return new WaitForSeconds(reappearDuration);
        }

        animator.SetTrigger("summonMines");
    }
    public void Teleport()
    {
        StartCoroutine(Teleportation());
    }

    private IEnumerator Teleportation()
    {
        animator.SetTrigger("disappear");
        yield return new WaitForSeconds(disappearDuration);

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
        animator.SetTrigger("reappear");
        yield return new WaitForSeconds(reappearDuration);

        animator.SetTrigger("idle");
        attacking = false;
    }

    public void SpawnMines()
    {
        if (phaseOne)
        {
            foreach (GameObject spawnLocation in phaseOneMineLocations)
            {
                GameObject spawnedMine = Instantiate(mine, spawnLocation.transform.position, Quaternion.identity);
                spawnedMine.GetComponent<Mine>().SetTarget(player);
            }
        }
        else if (phaseTwo)
        {
            foreach (GameObject spawnLocation in phaseTwoMineLocations)
            {
                GameObject spawnedMine = Instantiate(mine, spawnLocation.transform.position, Quaternion.identity);
                spawnedMine.GetComponent<Mine>().SetTarget(player);
            }
        }
        else if (phaseThree)
        {
            foreach (GameObject spawnLocation in phaseThreeMineLocations)
            {
                GameObject spawnedMine = Instantiate(mine, spawnLocation.transform.position, Quaternion.identity);
                spawnedMine.GetComponent<Mine>().SetTarget(player);
            }
        }
        Teleport();
    }
}
