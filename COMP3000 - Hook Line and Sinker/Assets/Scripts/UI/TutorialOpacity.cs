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

    public string keyboardPrompt;
    public string controllerPrompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = transform.Find("Image")?.GetComponent<Image>();
        text = transform.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();

        // Changes the tutorial message depending on the player's device.
        GameControl.gameControl.Load();
        if (GameControl.gameControl.device == GameControl.Device.Controller)
        {
            if (text != null && controllerPrompt != null)
            {
                text.text = controllerPrompt;
            }
        }
        else
        {
            if (text != null && keyboardPrompt != null)
            {
                text.text = keyboardPrompt;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Determines the distance between the tutorial message and the player.
        float distance = Vector2.Distance(player.transform.position, transform.position);
        // Alpha changes depending on the distance.
        // When the player is closer, the alpha is higher and vice versa.
        float alpha = Mathf.InverseLerp(maxDistance, minDistance, distance);

        // If the player has read the message and is now out of range, the message will be destroyed.
        if (alpha == 1f)
        {
            readByPlayer = true;
        }

        // Changes the background transparency to the value of alpha.
        if (image != null && alpha >= 0.85f)
        {
            Color imageColour = image.color;
            imageColour.a = 0.85f;
            image.color = imageColour;
        }
        else if (image != null && alpha < 0.85f)
        {
            Color imageColour = image.color;
            imageColour.a = alpha;
            image.color = imageColour;
        }

        // Changes the text transparency to the value of alpha.
        if (text != null && alpha >= 0.85f)
        {
            Color textcolour = text.color;
            textcolour.a = 0.85f;
            text.color = textcolour;
        }
        else if (text != null && alpha < 0.85f)
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
