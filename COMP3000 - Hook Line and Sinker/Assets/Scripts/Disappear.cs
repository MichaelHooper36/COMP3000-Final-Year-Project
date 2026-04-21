using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Disappear : MonoBehaviour
{
    private Tilemap tilemap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisappearObject()
    {
        // Finds the Tilemap component that contains the visuals for the debris.
        tilemap = transform.Find("Tilemap")?.GetComponent<Tilemap>();
        Debug.Log("DisappearObject called. Tilemap found: " + (tilemap != null));
        if (tilemap != null)
        {
            StartCoroutine(GoByeBye(1f));
        }
    }

    private IEnumerator GoByeBye(float duration)
    {
        float startingOpacity = tilemap.color.a;
        float elapsedTime = 0f;

        // Gradually reduces the tilemap's over time.
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newOpacity = Mathf.Lerp(startingOpacity, 0f, elapsedTime / duration);
            Color tilemapColour = tilemap.color;
            tilemapColour.a = newOpacity;
            tilemap.color = tilemapColour;
            yield return null;
        }

        // Once the tilemap is invisible, the object is destroyed.
        if (tilemap.color.a <= 0f)
        {
            BoxCollider2D collider = transform.parent.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            Destroy(gameObject);
        }
    }
}
