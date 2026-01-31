using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public GameObject player;
    public int damage;
    public RisingFloor risingFloor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == player.name)
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enemyDamageTimer = 0;
                playerMovement.TakeDamage(damage);

                if (playerMovement.currentHealth == playerMovement.maxHealth)
                {
                    if (risingFloor != null)
                    {
                        transform.position = new Vector2(transform.position.x, risingFloor.phaseOneY);
                    }
                    return;
                }
                else if (risingFloor != null && risingFloor.isRising)
                {
                    Transform safeGround = FindSafeGround();
                    playerMovement.transform.position = new Vector2(safeGround.position.x, safeGround.position.y + (safeGround.localScale.y / 2) + 2);
                }
                else 
                {
                    playerMovement.transform.position = new Vector2(playerMovement.previousGround.x, playerMovement.previousGround.y + 1);
                }

                playerMovement.movement = 0f;
                playerMovement.rigidBody.linearVelocityX = 0f;
                playerMovement.rigidBody.linearVelocityY = 0f;
                if (playerMovement.isGrappling)
                {
                    playerMovement.canMove = true;
                    playerMovement.canJump = true;
                    playerMovement.isGrappling = false;
                    playerMovement.distanceJoint.enabled = false;
                    playerMovement.lineRenderer.enabled = false;
                }
            }
        }
    }

    Transform FindSafeGround()
    {
        GameObject[] listOfPlatforms = GameObject.FindGameObjectsWithTag("Ground");
        Transform closestPlatform = null;
        float closestDistance = float.MaxValue;

        float floorY = risingFloor.transform.position.y;
        float ceilingY = risingFloor.ceilingBody.transform.position.y;

        foreach (GameObject ground in listOfPlatforms)
        {
            float groundY = ground.transform.position.y;
            float groundX = ground.transform.position.x;

            if (groundY > floorY + (risingFloor.transform.localScale.y / 2) + 1 && groundY < ceilingY && groundX > -48.5f && groundX < -29.5f)
            {
                float distance = Mathf.Abs(groundY - player.transform.position.y);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlatform = ground.transform;
                }
            }
        }
        Debug.Log(closestPlatform.position);
        return closestPlatform;
    }
}
