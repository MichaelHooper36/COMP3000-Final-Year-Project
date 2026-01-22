using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastDistance;

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

    private RaycastHit2D raycastHit;
    private Transform target;
    private float targetDistance;
    private bool inRange;
    private bool canMove;

    private Animator animator;
    private bool attacking;

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
            raycastHit = Physics2D.Raycast(rayCast.position, transform.right * transform.localScale.x, rayCastDistance, rayCastMask);
            RaycastDebugger();
        }

        if (raycastHit.collider != null)
        {
            canMove = false;
            EnemyLogic();
        }
        else if (raycastHit.collider == null)
        {
            inRange = false;
        }

        if (!inRange)
        {
            StopAttack();
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            target = collider.transform;
            inRange = true;
            Flip();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && WithinBorders())
        {
            if (transform.localScale.x < 0)
            {
                target = leftBoundry;
            }
            else
            {
                target = rightBoundry;
            }
        }
        else if (collider.gameObject.tag == "Player")
        {
            SelectTarget();
        }
    }

    void RaycastDebugger()
    {
        if (targetDistance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastDistance * transform.localScale.x, Color.red);
        }
        else if (targetDistance < attackDistance)
        {
            Debug.DrawRay(rayCast.position, transform.right * rayCastDistance * transform.localScale.x, Color.green);
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
        }
    }

    private bool WithinBorders()
    {
        return transform.position.x > leftBoundry.position.x && transform.position.x < rightBoundry.position.x;
    }

    private void SelectTarget()
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

    void Flip()
    {
        if (transform.position.x > target.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
            projectile.GetComponent<Projectile>().speed = -Mathf.Abs(projectile.GetComponent<Projectile>().speed);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
            projectile.GetComponent<Projectile>().speed = Mathf.Abs(projectile.GetComponent<Projectile>().speed);
        }
    }
}
