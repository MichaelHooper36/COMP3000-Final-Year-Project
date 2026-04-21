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
        if (other.gameObject.CompareTag("Player"))
        {
            // Reset the enemy damage timer and apply damage to the player
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
                    // If the ground is in the boss room, calls the FindSafeGround function.
                    Transform safeGround = FindSafeGround();
                    playerMovement.transform.position = new Vector2(safeGround.position.x, safeGround.position.y + (safeGround.localScale.y / 2) + 2);
                }
                else 
                {
                    // If the ground is not in the boss room, respawns the player at their previous grounded position.
                    playerMovement.transform.position = new Vector2(playerMovement.previousGround.x, playerMovement.previousGround.y + 1);
                }

                if (playerMovement.transform.rotation.y == 180)
                {
                    playerMovement.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    playerMovement.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                // Resets the player's velocity, to avoid the player being launched immediately after moving.
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
                    playerMovement.animator.SetBool("isGrappling", false);
                }
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }

    Transform FindSafeGround()
    {
        // Finds the closest safe ground platform above the rising floor that is within the boss room.
        GameObject[] listOfPlatforms = GameObject.FindGameObjectsWithTag("Ground");
        Transform closestPlatform = null;
        float closestDistance = float.MaxValue;

        float floorY = risingFloor.transform.position.y;

        foreach (GameObject ground in listOfPlatforms)
        {
            float groundY = ground.transform.position.y;
            float groundX = ground.transform.position.x;

            if (groundY > floorY + (risingFloor.transform.localScale.y / 2) + 1 && groundX > -48.5f && groundX < -29.5f)
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
