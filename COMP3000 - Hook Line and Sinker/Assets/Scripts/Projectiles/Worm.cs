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
        elapsedTime += Time.deltaTime;
        float offset = Mathf.Lerp(minUp, maxUp, (Mathf.Sin(elapsedTime * alternationSpeed) + 1) / 2);
        rigidBody.linearVelocity = (Vector2)(transform.right * speed) + (Vector2)(transform.up * offset);
    }
}
