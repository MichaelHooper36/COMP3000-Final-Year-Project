using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    // References to other scripts.
    public CloseDoor closeDoor;
    public RisingFloor risingFloor;
    public PauseMenu pauseMenu;

    // Visual elements.
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColour;
    
    // References to player and mine objects.
    public GameObject player;
    public GameObject mine;
    public GameObject[] phaseOneMineLocations;
    public GameObject[] phaseTwoMineLocations;
    public GameObject[] phaseThreeMineLocations;
    
    // Health and invulnerability.
    public int maxHealth;
    public int currentHealth;
    public GameObject bossHealth;
    public HealthBar healthBar;
    public bool invulnerable;

    // Determining what phase the boss is in.
    public bool phaseOne;
    public bool phaseTwo;
    public bool phaseThree;
    public bool dead;

    // Teleportation and attack timing.
    public float teleportInterval = 4f;
    public float teleportTimer;
    public bool attacking;
    public float disappearDuration = 2f;
    public float reappearDuration = 2f;

    // Spawn locations.
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
        // Initialize health and phase states.
        currentHealth = maxHealth;
        phaseOne = true;
        attacking = false;
        invulnerable = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColour = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Only starts the boss fight once the player goes through the door.
        if (closeDoor.isClosed && !dead)
        {
            bossHealth.SetActive(true);
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            if (!attacking)
            {
                // Countdown to the next teleport and attack.
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
        if (!invulnerable)
        {
            // Reduces health by the damage taken and flashes red.
            currentHealth -= damage;
            StartCoroutine(DamageFlash());
            healthBar.SetCurrentHealth(currentHealth);
            if (currentHealth <= 0)
            {
                // If the boss dies, opens the door, starts the rising floor, and saves the time taken for the level.
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
            else if (currentHealth <= maxHealth / 2 && phaseTwo)
            {
                // Transitions to phase three, starting the rising floor and teleporting to the phase three attack zone.
                phaseThree = true;
                phaseTwo = false;
                risingFloor.isRising = true;
                risingFloor.playerAtNextPhase = false;

                StopAllCoroutines();
                attacking = false;

                StartCoroutine(PhaseTransition(phaseThreeAttackZone.transform));
            }
            else if (currentHealth <= maxHealth * 3 / 4 && phaseOne)
            {
                // Transitions to phase two, starting the rising floor and teleporting to the phase two attack zone.
                phaseOne = false;
                phaseTwo = true;
                risingFloor.isRising = true;
                risingFloor.playerAtNextPhase = false;

                StopAllCoroutines();
                attacking = false;

                StartCoroutine(PhaseTransition(phaseTwoAttackZone.transform));
            }
        }
    }

    private IEnumerator PhaseTransition(Transform newPosition)
    {
        // Plays the disappear and reappear animations, before teleporting to the new position.
        spriteRenderer.color = originalColour;
        invulnerable = true;
        animator.SetTrigger("disappear");
        yield return new WaitForSeconds(disappearDuration);
        
        transform.position = newPosition.position;

        animator.SetTrigger("reappear");
        yield return new WaitForSeconds(reappearDuration);

        animator.SetTrigger("idle");
        invulnerable = false;
    }

    private IEnumerator MineSpawner()
    {
        // Teleports to the attack zone in the current phase and starts the mine spawning animation.
        // Animation called the SpawnMines function.
        invulnerable = true;
        if (phaseOne && Vector2.Distance(transform.position, phaseOneAttackZone.transform.position) > 2f)
        {
            animator.SetTrigger("disappear");
            yield return new WaitForSeconds(disappearDuration);

            transform.position = phaseOneAttackZone.transform.position;

            animator.SetTrigger("reappear");
            yield return new WaitForSeconds(reappearDuration);
        }
        else if (phaseTwo && Vector2.Distance(transform.position, phaseTwoAttackZone.transform.position) > 2f)
        {
            animator.SetTrigger("disappear");
            yield return new WaitForSeconds(disappearDuration);

            transform.position = phaseTwoAttackZone.transform.position;

            animator.SetTrigger("reappear");
            yield return new WaitForSeconds(reappearDuration);
        }
        else if (phaseThree && Vector2.Distance(transform.position, phaseThreeAttackZone.transform.position) > 2f)
        {
            animator.SetTrigger("disappear");
            yield return new WaitForSeconds(disappearDuration);

            transform.position = phaseThreeAttackZone.transform.position;

            animator.SetTrigger("reappear");
            yield return new WaitForSeconds(reappearDuration);
        }
        invulnerable = false;
        animator.SetTrigger("summonMines");
    }
    public void Teleport()
    {
        StartCoroutine(Teleportation());
    }

    private IEnumerator Teleportation()
    {
        // Plays the disappear and reappear animations, before teleporting to a spawn location in the current phase.
        invulnerable = true;
        animator.SetTrigger("disappear");
        yield return new WaitForSeconds(disappearDuration);

        if (phaseThree)
        {
            // Randomly selects a phase three spawn location, excluding the one closest to the player, and teleports to it.
            Vector3 closestSpawn = Vector3.zero;
            foreach (GameObject spawnLocation in phaseThreeSpawns)
            {
                if (spawnLocation != null)
                {
                    Vector2 spawnPosition = spawnLocation.transform.position;
                    Vector2 playerPosition = player.transform.position;
                    float distance = Vector2.Distance(spawnPosition, playerPosition);
                    if (distance < Vector2.Distance(closestSpawn, playerPosition))
                    {
                        closestSpawn = spawnPosition;
                    }
                }
            }
            List<GameObject> validSpawns = new List<GameObject>();
            foreach (GameObject spawnLocation in phaseThreeSpawns)
            {
                if (spawnLocation.transform.position != closestSpawn)
                {
                    validSpawns.Add(spawnLocation);
                }
            }
            if (validSpawns.Count > 0)
            {
                GameObject selectedSpawn = validSpawns[Random.Range(0, validSpawns.Count)];
                transform.position = selectedSpawn.transform.position;
            }
            else
            {
                Debug.LogWarning("No valid spawn locations found in phaseThreeSpawns.");
                transform.position = closestSpawn;
            }
        }
        else if (phaseTwo)
        {
            // Randomly selects a phase two spawn location, excluding the one closest to the player, and teleports to it.
            Vector3 closestSpawn = Vector3.zero;
            foreach (GameObject spawnLocation in phaseTwoSpawns)
            {
                if (spawnLocation != null)
                {
                    Vector2 spawnPosition = spawnLocation.transform.position;
                    Vector2 playerPosition = player.transform.position;
                    float distance = Vector2.Distance(spawnPosition, playerPosition);
                    if (distance < Vector2.Distance(closestSpawn, playerPosition))
                    {
                        closestSpawn = spawnPosition;
                    }
                }
            }

            List<GameObject> validSpawns = new List<GameObject>();
            foreach (GameObject spawnLocation in phaseTwoSpawns)
            {
                if (spawnLocation.transform.position != closestSpawn)
                {
                    validSpawns.Add(spawnLocation);
                }
            }

            if (validSpawns.Count > 0)
            {
                GameObject selectedSpawn = validSpawns[Random.Range(0, validSpawns.Count)];
                transform.position = selectedSpawn.transform.position;
            }
            else
            {
                Debug.LogWarning("No valid spawn locations found in phaseTwoSpawns.");
                transform.position = closestSpawn;
            }
        }
        else
        {
            // Randomly selects a phase one spawn location, excluding the one closest to the player, and teleports to it.
            Vector3 closestSpawn = Vector3.zero;
            foreach (GameObject spawnLocation in phaseOneSpawns)
            {
                if (spawnLocation != null)
                {
                    Vector2 spawnPosition = spawnLocation.transform.position;
                    Vector2 playerPosition = player.transform.position;
                    float distance = Vector2.Distance(spawnPosition, playerPosition);
                    if (distance < Vector2.Distance(closestSpawn, playerPosition))
                    {
                        closestSpawn = spawnPosition;
                    }
                }
            }
            List<GameObject> validSpawns = new List<GameObject>();
            foreach (GameObject spawnLocation in phaseOneSpawns)
            {
                if (spawnLocation.transform.position != closestSpawn)
                {
                    validSpawns.Add(spawnLocation);
                }
            }
            if (validSpawns.Count > 0)
            {
                GameObject selectedSpawn = validSpawns[Random.Range(0, validSpawns.Count)];
                transform.position = selectedSpawn.transform.position;
            }
            else
            {
                Debug.LogWarning("No valid spawn locations found in phaseOneSpawns.");
                transform.position = closestSpawn;
            }
        }

        animator.SetTrigger("reappear");
        yield return new WaitForSeconds(reappearDuration);

        animator.SetTrigger("idle");
        invulnerable = false;
        attacking = false;
    }

    public void SpawnMines()
    {
        // Spawns a mine at each spawn location in the current phase.
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

    private IEnumerator DamageFlash()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = originalColour;
            yield return new WaitForSeconds(0.125f);
        }
    }
}
