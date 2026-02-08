using UnityEngine;

public class OutOfWater : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rigidBody = collider.GetComponent<Rigidbody2D>();
            rigidBody.gravityScale = 2.0f;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rigidBody = collider.GetComponent<Rigidbody2D>();
            rigidBody.gravityScale = 1.5f;
        }
    }
}
