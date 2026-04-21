using UnityEngine;

public class Worm : Projectile
{
    public float minUp = -1f;
    public float maxUp = 1f;
    public float alternationSpeed = 2f;

    private float elapsedTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // Moves the projectile up and down to simulate a worm-like movement.
        elapsedTime += Time.deltaTime;
        float offset = Mathf.Lerp(minUp, maxUp, (Mathf.Sin(elapsedTime * alternationSpeed) + 1) / 2);
        if (rigidBody.linearVelocityX < 0)
        {
            rigidBody.linearVelocity = (Vector2)(transform.right * speed) + (Vector2)(-transform.up * offset);
        }
        else
        {
            rigidBody.linearVelocity = (Vector2)(transform.right * speed) + (Vector2)(transform.up * offset);
        }
    }
}
