using UnityEngine;

public class Mine : MonoBehaviour
{
    public Vector2 speed;
    protected Rigidbody2D rigidBody;

    public int damage = 20;
    public float elapsedTime = 0f;

    private PlayerMovement playerMovement;
    private GameObject target;
    public LayerMask playerLayer;
    private bool inRange = false;

    public Animator mineAnim;
    public Animator radiusAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // When the target is set by the boss, the mine will float towards it.
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rigidBody.linearVelocity = direction * speed.magnitude;
        }

        // If the player is within range of the mine, the timer animation will start.
        inRange = Physics2D.OverlapCircle(transform.position, 3f, playerLayer);
        if (inRange)
        {
            radiusAnim.SetBool("inRange", true);
            if (target != null)
            {
                playerMovement = target.GetComponent<PlayerMovement>();
            }
        }
        else
        {
            playerMovement = null;
        }

        // The mine will explode after enough time, even if the player is not in range.
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 7.5f)
        {
            mineAnim.SetBool("isDestroyed", true);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    // Called by the animation. If the player is in range, the player will take damage.
    public void Explode()
    {
        if (inRange && playerMovement != null)
        {
            playerMovement.TakeDamage(damage);
        }
    }

    // Called by the animation, destroys the mine.
    public void DestroyMine()
    {
        Destroy(gameObject);
    }
}
