using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour
{
    // Parameters for moving between two points
    public Transform leftBoundry;
    public Transform rightBoundry;
    public float movementSpeed;

    // Parameters for attacking the player
    public GameObject projectile;
    public Transform firePoint;
    public float attackDistance;
    public float attackCooldown;
    private float initialTimer;
    private bool onCooldown;

    // Health
    public float maxHealth;
    public float currentHealth;

    // Determining target and range
    [HideInInspector] public Transform target;
    private float targetDistance;
    [HideInInspector] public bool inRange;
    private bool canMove;
    public GameObject hotZone;
    public GameObject triggerArea;

    // Animation and visual feedback
    private Animator animator;
    public bool attacking;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer headSprite;
    private Color originalColor;

    void Awake()
    {
        // Initialize variables and set the initial target.
        SelectTarget();
        onCooldown = true;
        initialTimer = attackCooldown;
        attackCooldown = 0;
        currentHealth = maxHealth;
        canMove = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Animator and SpriteRenderer components.
        spriteRenderer = GetComponent<SpriteRenderer>();
        headSprite = firePoint.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle attack cooldown.
        if (onCooldown)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown < 0)
            {
                attackCooldown = 0;
                onCooldown = false;
            }
        }

        if (inRange)
        {
            EnemyLogic();
        }

        if (!attacking)
        {
            Move();
        }

        if (!WithinBorders() && !inRange)
        {
            SelectTarget();
        }
    }

    void EnemyLogic()
    {
        // Calculate the distance to the target and determine whether to attack or stop attacking.
        targetDistance = Vector2.Distance(transform.position, target.position);

        if (targetDistance > attackDistance)
        {
            StopAttack();
        }
        else if (targetDistance <= attackDistance && onCooldown == false) 
        {

            Debug.Log("Fish attacking.");
            FireProjectile();
        }
    }

    void Move()
    {
        // Move towards the target.
        if (canMove)
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }

    void FireProjectile() 
    {
        // If the attack is not on cooldown, start the attack sequence.
        if (attackCooldown == 0)
        {
            attacking = true;

            int i = 0;
            StartCoroutine(Shoot(i));

            onCooldown = true;
            attackCooldown = initialTimer;
            Debug.Log("Fish fired projectile.");
            canMove = true;
        }
    }

    private IEnumerator Shoot(int i)
    {
        // Shoot a projectile towards the target, then wait and repeat until 3 projectiles have been fired.
        if (i < 3)
        {
            Vector2 fireDirection = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            if (Mathf.Approximately(transform.eulerAngles.y, 180f))
            {
                firePoint.rotation = Quaternion.Euler(new Vector3(0, 180, 180 - angle));
            }
            else
            {
                firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            firePoint.position = (Vector2)transform.position + new Vector2(fireDirection.x * 0.04f, fireDirection.y * 0.017f);
            GameObject firedProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
            firedProjectile.GetComponent<Projectile>().SetOrigin(gameObject);
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(Shoot(i + 1));
        }
    }

    void StopAttack()
    {
        attacking = false;
        canMove = true;
    }

    public void TakeDamage(int damage)
    {
        // Reduce health by the damage amount, flash red, and destroy the fish if health drops to 0 or below.
        if (damage > 0)
        {
            currentHealth -= damage;
            StartCoroutine(DamageFlash());
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
            // If a player hits the fish, the fish will select the player as a target and flip to face them.
            Flip();
        }
    }

    private bool WithinBorders()
    {
        // Check if the fish is within the left and right boundaries.
        return transform.position.x > leftBoundry.position.x && transform.position.x < rightBoundry.position.x;
    }

    public void SelectTarget()
    {
        // Select the target based on which boundary is farther from the fish's current position.
        float distanceToLeft = Vector2.Distance(transform.position, leftBoundry.position);
        float distanceToRight = Vector2.Distance(transform.position, rightBoundry.position);

        if (distanceToLeft >= distanceToRight)
        {
            target = leftBoundry;
        }
        else if (distanceToRight >= distanceToLeft)
        {
            target = rightBoundry;
        }
        else
        {
            target = null;
        }

        Flip();
    }

    public void Flip()
    {
        // Flip the fish to face the target.
        if (transform.position.x > target.position.x)
        {
            transform.eulerAngles = new Vector2(0, 180);
            firePoint.transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 0);
            firePoint.transform.eulerAngles = new Vector2(0, 0);
        }
    }

    private IEnumerator DamageFlash()
    {
        // Flashes red twice when the fish takes damage.
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            headSprite.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = originalColor;
            headSprite.color = originalColor;
            yield return new WaitForSeconds(0.125f);
        }
    }
}
