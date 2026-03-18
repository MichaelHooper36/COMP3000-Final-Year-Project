using UnityEngine;

public class Mine : MonoBehaviour
{
    public Vector2 speed;
    protected Rigidbody2D rigidBody;

    public int damage = 20;

    private PlayerMovement playerMovement;
    private GameObject target;
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
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rigidBody.linearVelocity = direction * speed.magnitude;
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == target)
        {
            inRange = true;
            playerMovement = collider.GetComponent<PlayerMovement>();
            radiusAnim.SetBool("inRange", true);
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == target)
        {
            inRange = false;
        }
    }

    public void Explode()
    {
        if (inRange && playerMovement != null)
        {
            playerMovement.TakeDamage(damage);
        }
    }

    public void DestroyMine()
    {
        Destroy(gameObject);
    }
}
