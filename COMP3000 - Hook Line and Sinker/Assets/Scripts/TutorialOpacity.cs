using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialOpacity : MonoBehaviour
{
    public GameObject player;
    public float maxDistance = 20f;
    public float minDistance = 10f;
    private bool readByPlayer = false;

    private Image image;
    private TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = transform.Find("Image")?.GetComponent<Image>();
        text = transform.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        float alpha = Mathf.InverseLerp(maxDistance, minDistance, distance);

        if (alpha == 1f)
        {
            readByPlayer = true;
        }

        if (image != null)
        {
            Color imageColour = image.color;
            imageColour.a = alpha;
            image.color = imageColour;
        }

        if (text != null)
        {
            Color textColour = text.color;
            textColour.a = alpha;
            text.color = textColour;
        }

        if (readByPlayer && alpha == 0f)
        {
            Destroy(gameObject);
        }
    }
}
