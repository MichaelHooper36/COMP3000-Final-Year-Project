using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastDistance;

    public GameObject projectile;
    public Transform firePoint;
    public float attackDistance;
    public float movementSpeed;
    public float attackCooldown;
    private float initialTimer;
    private bool onCooldown;

    public float maxHealth;
    public float currentHealth;

    private RaycastHit2D raycastHit;
    private GameObject target;
    private float targetDistance;
    private bool inRange;

    private Animator animator;
    private bool attacking;

    void Awake()
    {
        onCooldown = true;
        initialTimer = attackCooldown;
        attackCooldown = 0;
        currentHealth = maxHealth;
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
            raycastHit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastDistance, rayCastMask);
            RaycastDebugger();
        }

        if (raycastHit.collider != null)
        {
            EnemyLogic();
        }
        else if (raycastHit.collider == null)
        {
            inRange = false;
        }

        if (inRange == false)
        {
            StopAttack();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            target = collider.gameObject;
            inRange = true;
        }
    }

    void RaycastDebugger()
    {
        if (targetDistance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastDistance, Color.red);

        }
        else if (targetDistance < attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastDistance, Color.green);

        }
    }

    void EnemyLogic()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);

        if (targetDistance > attackDistance)
        {
            Move();
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
        Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }

    void FireProjectile() 
    {
        if (attackCooldown == 0)
        {
            attacking = true;
            Instantiate(projectile, firePoint.position, firePoint.rotation);
            onCooldown = true;
            attackCooldown = initialTimer;
            Debug.Log("Fish fired projectile.");
        }
    }

    void StopAttack()
    {
        onCooldown = false;
        attacking = false;
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
        }
    }
}
