using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public GameObject player;
    public int damage;

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
                playerMovement.transform.position = new Vector2(playerMovement.previousGround.x, playerMovement.previousGround.y + 1);
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
}
