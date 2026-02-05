using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public Transform leftBoundry;
    public Transform rightBoundry;
    public float movementSpeed;

    public GameObject projectile;
    public Transform firePoint;
    public float attackDistance;
    public float attackCooldown;
    private float initialTimer;
    private bool onCooldown;

    public float maxHealth;
    public float currentHealth;

    [HideInInspector] public Transform target;
    private float targetDistance;
    [HideInInspector] public bool inRange;
    private bool canMove;
    public GameObject hotZone;
    public GameObject triggerArea;

    private Animator animator;
    public bool attacking;

    void Awake()
    {
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
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if (canMove)
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }

    void FireProjectile() 
    {
        if (attackCooldown == 0)
        {
            Vector2 fireDirection = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            firePoint.position = (Vector2)transform.position + fireDirection * 0.75f;

            attacking = true;
            Instantiate(projectile, firePoint.position, firePoint.rotation);
            onCooldown = true;
            attackCooldown = initialTimer;
            Debug.Log("Fish fired projectile.");
            canMove = true;
        }
    }

    void StopAttack()
    {
        attacking = false;
        canMove = true;
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            currentHealth -= damage;
            Debug.Log("Fish current health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
            Flip();
        }
    }

    private bool WithinBorders()
    {
        return transform.position.x > leftBoundry.position.x && transform.position.x < rightBoundry.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftBoundry.position);
        float distanceToRight = Vector2.Distance(transform.position, rightBoundry.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftBoundry;
        }
        else if (distanceToRight > distanceToLeft)
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
        if (transform.position.x > target.position.x)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
    }
}
